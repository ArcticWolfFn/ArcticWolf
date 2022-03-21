using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ArcticWolf.Apis.Base.Common.Json;
using ArcticWolf.DataMiner.Models.Discord;

namespace ArcticWolf.DataMiner.Managers;

public class HotFixManager
{
    public static void LoadHotFixesFromMessages()
        {
            var dbContext = Program.DbContext;
        
            Log.Information("Loading data...", "HotfixLoader");
        
            string jsonData;
            try
            {
                jsonData = File.ReadAllText(Program.Configuration.HotfixDiscordChatHistoryFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while reading the hotfix chat history: " + ex.Message, "HotfixLoader");
                return;
            }
        
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Log.Error("An error occured while reading the hotfix chat history: File is empty", "HotfixLoader");
                return;
            }
        
            ChatHistory chatHistory;
            try
            {
                chatHistory = JsonDeserializer.Deserialize<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while parsing the hotfix chat history: " + ex.Message, "HotfixLoader");
                return;
            }

            if (chatHistory == null)
            {
                Log.Warning("No chat history available.", "HotfixLoader");
                return;
            }
        
            Log.Information($"Parsing hotfix data from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", "HotfixLoader");
        
            foreach (Message msg in chatHistory.Messages)
            {
                if (msg.Attachments.Count < 1)
                {
                    continue;
                }
        
                foreach (var attachment in msg.Attachments)
                {
                    Log.Debug($"Loading file '{attachment.Url}'", "HotfixLoader");
        
                    string hotFixFileContent;
                    try
                    {
                        hotFixFileContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Program.Configuration.HotfixDiscordChatHistoryFilePath), attachment.Url));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("An error occured while parsing a hotfix file: " + ex.Message, "HotfixLoader");
                        continue;
                    }
        
                    if (string.IsNullOrWhiteSpace(hotFixFileContent))
                    {
                        Log.Verbose("Skipping file, because it's empty", "HotfixLoader");
                        continue;
                    }
        
                    // Category Regex
                    var categoryMatches = Regex.Matches(hotFixFileContent, @"\[.*\]\s");
        
                    foreach (Match categoryMatch in categoryMatches)
                    {
                        var nextMatch = categoryMatch.NextMatch();
                        var categoryName = categoryMatch.Value.Replace('\r', ' ').Replace('\n', ' ').Replace('[', ' ').Replace(']', ' ').Trim();
        
                        var categoryEndIndex = nextMatch.Index;
                        if (categoryEndIndex == 0)
                        {
                            categoryEndIndex = hotFixFileContent.Length;
                            Log.Debug("This is the last category of this hotfix file.", "HotfixLoader");
                        }
        
                        Log.Debug($"Found hotfix category '{categoryName}', starting at index {categoryMatch.Index} and ending at index {categoryEndIndex}");
        
                        var categoryStartIndex = categoryMatch.Index + categoryMatch.Length;
        
                        var categoryContent = hotFixFileContent.Substring(categoryStartIndex, categoryEndIndex - categoryStartIndex);
        
                        var variableMatches = Regex.Matches(hotFixFileContent, @".*=.*(\r\n|\r|\n|$)");
        
                        foreach(Match variableMatch in variableMatches)
                        {
                            var variableContent = variableMatch.Value.Replace('\r', ' ').Replace('\n', ' ').Trim();
        
                            // remove the diff prefix
                            if (variableContent.StartsWith("+ ") || variableContent.StartsWith("- "))
                            {
                                variableContent = variableContent[2..];
                            }
        
                            var variableNameMatch = Regex.Matches(variableContent, @"([^=])*=").First();
                            var variableName = variableNameMatch.Value.Replace("=", "").Replace("+", "").Replace("-", "");
        
                            Log.Debug($"Found variable: '{variableName}'", "HotfixLoader");
        
                            var variableValue = variableContent.Replace(variableNameMatch.Value, "");
        
                            Log.Debug($"Found variable value: '{variableValue}'", "HotfixLoader");
        
                            if (variableValue.StartsWith("(") && variableValue.EndsWith(")"))
                            {
                                // Variable has multi params
        
                                // remove first and last parenthesis
                                variableValue = variableValue[1..];
                                variableValue = variableValue.Remove(variableValue.Length - 1);
        
                                var variableParamMatches = Regex.Matches(variableValue, @"\w*=(\((\(.*\))*?\)|\(.*\)|\"".*?\""|\w*)");
        
                                foreach (Match paramMatch in variableParamMatches)
                                {
                                    var paramName = Regex.Match(paramMatch.Value, @"\w*=").Value;
                                    paramName = paramName.Remove(paramName.Length - 1); // remove '=' sign
        
                                    var paramValue = Regex.Match(paramMatch.Value, @"=.*").Value[1..]; // remove '=' sign
        
                                    Log.Debug($"Found variable param '{paramName}' with value of '{paramValue}'", "HotfixLoader");
                                }
                            }
                            else // Variable Value
                            {
                                Log.Debug($"Found variable value: '{variableValue}'", "HotfixLoader");
                            }
                        }
                    }
        
                    Thread.Sleep(3000);
                }
            }
        }
}