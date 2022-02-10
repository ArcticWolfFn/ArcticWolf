# ArcticWolf | Fortnite DataMiner, Mod & Bot

## Current State
I am currently not actively working on this project anymore. The Core worked in a previous version, but after I refactored the code to create a framework
the game crashes due to a memory exception and I can't fix it currently.

## Projects and their purpose
| Project | Purpose | Note |
|---------|---------|------|
| [ArcticWolf.Core](/ArcticWolf.Core/) | Fortnite modifications, which makes it possible to play v15.00 offline. Based on Neonite++ (but with better performance). | Fortnite crashes after starting a match due to code refactoring. |
| [ArcticWolf.DataMiner.Models](/ArcticWolf.DataMiner.Models/) | Contains models used for the data miner | |
| [ArcticWolf.DataMiner](/ArcticWolf.DataMiner/) | Collects data from various APIs and stores data inside a custom database | |
| [ArcticWolf.Storage](ArcticWolf.Storage) | Contains models and the database system used by the API and the data miner | |
| [ArcticWolfApi](/ArcticWolfApi/) | Fortnite and EpicGames API clone used by ArcticWolf.Core to send custom data | I was too lazy to develop this myself so I used the code from the Rift project|
| [ArcticWolfLauncher](/ArcticWolfLauncher/) | Launches the specified Fortnite version with fake anti cheat and injects the ArcticWolf.Core | |
| [Discord/BotCord](/Discord/BotCord/) | Simple framework for Discord bots | |
| [Discord/FNitePlusBot](/Discord/FNitePlusBot/) | Bot for interacting with Fortnite-related data | |
| [Modules/Logging](/Modules/Logging/) | Custom and easy-to-use logger | |
