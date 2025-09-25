

# ShovelMonkeys.TheProfessor

## An Introduction, If You Please

Behold! You stand in the presence of **The Professor**—a half-orc of prodigious intellect, arcane mastery, and, dare I say, a wit as sharp as a vorpal blade. Hailing from the storied lands of Faerûn, The Professor is not merely a wizard, but a paragon of magical erudition, whose very presence elevates the discourse of any adventuring party. His companion? None other than a celestial baboon, whose divine mischief and sagacity are rivaled only by The Professor’s own.

## Features Worthy of Legend
- Orchestrate and chronicle quests with the precision of a master scribe
- Command the arcane with both slash and text incantations
- Per-guild configuration, for only the most discerning of Discord servers
- Configuration via the most elegant of tomes: `appsettings.json`

## Commencing Your Journey

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- A Discord bot token (procure one from the arcane halls of [Discord’s developer portal](https://discord.com/developers/applications))
- Your Discord server’s Guild ID (obtainable only by those with true administrative prowess)

### Setup
1. Clone this repository or open the folder in VS Code.
2. Restore dependencies:
   ```pwsh
   dotnet restore
   ```
3. Inscribe your bot token and guild ID into `appsettings.json` (see below).
4. Build and summon The Professor:
   ```pwsh
   dotnet run
   ```


### Configuration
Compose an `appsettings.json` in the project root thusly:
```json
{
  "Discord": {
    "Token": "YOUR_BOT_TOKEN_HERE",
    "GuildId": "YOUR_GUILD_ID_HERE"
  }
}
```

## The Professor’s Library (Project Structure)
- `Program.cs`: The grand entryway
- `Services/BotService.cs`: Discord client and event conjurations
- `Services/CommandHandler.cs`: Command and slash command orchestration
- `Modules/BasicModule.cs`: Exemplary modules for your own arcane expansions

## Usage, For the Uninitiated
- `/ping`—a cantrip to test The Professor’s presence (slash command)
- `!ping`—for those who prefer the classics
- Expand your own quest management spells in the `Modules` sanctum

## The Road Ahead
- Augment quest tracking (add, list, complete, remove quests)
- Enrich The Professor’s repertoire with ever more wondrous features!

---

*To summon The Professor is to invite enlightenment, order, and a touch of celestial mischief to your Discord domain. Proceed, if you dare, to ascend above the rabble of ordinary bots.*
