# v1.0.8
- Changed most of the UI element code to be better aligned and scaled
  - Also modified the character and word spacing to be nicer
  - I also changed which HUD element my UI elements are parented to - It is now higher in the object hierarchy and directly under the "IngamePlayerHUD" object.
- Added layout config options - You can now set the following:
  - Change the X (Left/Right) position and Scaling
  - Change the Y (Up/Down) position and Scaling
  - Change the Z (Tilt) rotation
  - Change the text field width offset (allows it to show more or less text before truncating)
- Added font config options - You can now set the following:
  - Moved All Caps from the General section into this section
  - Change the font - I have added a font bundle for the 3 current versions of the "3270Fonts" font that the game uses (bundle includes embedded license and I added the license to my mod package)
    - Vanilla = Normal game font
    - Fixed = Currently available version of the font (fixes the dollar sign and other special characters)
    - Fixed-Semi-Condensed = Same as *Fixed* but slightly more condensed
    - Fixed-Condensed = Same as *Fixed* but more condensed
    > **Note:** If you are a mod developer and want to use this font asset bundle, please read the **"Font Asset Bundle Information"** section in the readme
  - Change the font size - **_IMPORTANT_**: I must stress that changing the font size could cause the layout spacing to have unwanted outcomes
  - Change the character spacing
  - Change the word spacing
  - Change the text element alignments - **_IMPORTANT_**: I must stress that changing this will **DEFINITELY** have unwanted outcomes to the layout
  - Change the text transparency (alpha)
- Changed how short data points are handled
  - You can now make any data point short by adding a ":#" inside the percent signs (e.g. ```%Weather:4%```)
  - Renamed ```%MoonLongName%``` to ```%MoonName%```
  - Removed the ```Short Character Count``` config setting
  - Removed the ```%MoonShortName%``` and the ```%WeatherShort%``` data points
- Changed how the Deadline color coding works
  - There is now a whole section for the Deadline data point
  - You can enable/disable the color coding
  - You can enable/disable replacing zero with a custom string (Default: ```<b>NOW!</b>```)
  - Set the color for each threshold (2+ days left, 1 day left, 0 days left)
  - Removed the ```%DeadlineWithColors%``` data point
- Added a config section for the MoonName data point
  - You can enable/disable showing the full moon name (e.g. ```21 Offense``` vs ```Offense```)
  - You can enable/disable replacing the company moons name with a custom string (Default: ```Company Building```)
- Added a clear/none weather custom text string configuration - Make it blank to keep it as None (Default: ```Clear```)
- Added color coding with customization for the Weather data point
  - You can enable/disable color coding for the ```%Weather%``` data point
    - Support was added for the [HellWeather](https://thunderstore.io/c/lethal-company/p/stormytuna/HellWeather/) mod so its 'Hell' weather variant can have its own color
  - You can set a custom color for each type of weather
- Added a configuration converter so that old settings get migrated to new settings
  - This includes automatically converting any short data point that was used before to the new format using the old short character count value
  - This should also clean up the config file so there are no orphaned config entries
- Since the layout changes made the scaling better and more concise I have changed the following default Line formats:
  - Line #1: ```Ship: $%ShipLootValue%(%ShipLootCount%) / $%MoonLootValue%(%MoonLootCount%) <i>[%MoonName%:%Weather%]</i>```
  - Line #2: ```Quota: $%FulfilledValue% / $%QuotaValue% - Profit: $%ExpectedProfit%(%CompanyRate%%)```

# v1.0.7
- Fixed an [issue](https://github.com/ProfX66/ShipLootPlus/issues/6) where a players dead body counted in the scrap calculation, they are now ignored.
- Fixed a rare error when leaving a lobby after having just scanned causing the mod to stop working until a game restart

# v1.0.6
- Fixed a compatibility [issue](https://github.com/ProfX66/ShipLootPlus/issues/5) with [MelonLoader](https://thunderstore.io/c/lethal-company/p/BepInEx/BepInEx_MLLoader) where it would throw an error causing ShipLootPlus to not load correctly
- Changed how the original ShipLoot is handled if its found, ShipLootPlus will now disable the original if its found

# v1.0.5
- Actually fixed joining a server as a client mid-round with scrap already on the ship not calculating correctly for the joined player
- Fixed scrap values not updating correctly when placing scrap items on the counter at the company
- Balanced the rate limiting to make it feel better when interacting with scrap

# v1.0.4
- Fixed an [issue](https://github.com/ProfX66/ShipLootPlus/issues/3) where joining a server as a client mid-round with scrap already on the ship was not calculating correctly for the joined player.
- Fixed an [issue](https://github.com/ProfX66/ShipLootPlus/issues/4) where spamming the grab button many times a second causing the same issues as the perpetual scan which caused a game crash (buffer overrun).
- Fixed an issue where selling items at the company would'nt refresh the data on occasion
- Fixed an issue where moon scrap value/count wasn't updating for all players on level load until the ship had fully landed
- Fixed patching methods that were some how not actually patching correctly
  - I also deep dived the hooked events and realized that I could remove some after I got these patches to actually apply
- Changed the refresh method to be rate limited so if an event wants to update the values but the values are already being refreshed it will skip triggering another refresh (this should add performance in the situations where multiple hooked events trigger a data refresh concurrently)
- Removed a left over ForceMeshUpdate() call which was not needed

# v1.0.3
- Hopefully fixed a compatibility issue with mods that modify the scanner to be able to perpetually scan (like [Hold Scan Button](https://thunderstore.io/c/lethal-company/p/FutureSavior/Hold_Scan_Button/))
- Added a new configuration section for "On Scan"
  - Moved "Display Duration" into this new section
  - Added "Reset Duration Timer On Scan" option which will reset the timer of the UI when a scan is initialized, which keeps the UI always on screen if scanning is active (defaults to disabled).
  - Added "Refresh Data On Scan" option which will force a data update on scan, this is not really needed as all data is updated via event triggers, but it was left in the last two releases as a way to force refresh on demand. This is what is incompatible with mods like [Hold Scan Button](https://thunderstore.io/c/lethal-company/p/FutureSavior/Hold_Scan_Button/) (defaults to disabled).
- Fixed an issue where moon scrap count was showing as 1 item while in space.

# v1.0.2
- Fixed an issue where "Always On" mode did not function correctly when "Allow Outside" and/or "Allow Inside" was disabled.
- Fixed an issue where the "On Scan" timeout didn't get reset when scanning again
- Updated readme with better Rich Text resource links (thanks persondatlovesgames)

# v1.0.1
> **_IMPORTANT_**: With **v1.0.1** there has been some changes which will reset any custom line formats and colors. You do not need to regenerate your configs and these settings will still be in your config file, its just that the line names and categories have changed (though now you would have extra data in the files).

- Refactored and abstracted most of the code to be more dynamic, performant, and reliable
- Optimized the calculation methods for getting scrap items and values

##
- Changed how the data is refreshed, it is now event driven so that the data is always updated even when not being shown
  - I only hooked events that affect the data point values but it will also trigger a refresh on scan. The end result is the data will always be current.
- Changed LethalConfig to a soft dependency so it can safely be removed (if desired) and not break ShipLootPlus
  - The Thunderstore package will still have it as a dependency as using it is the intended way to configure ShipLootPlus
- Changed the ```%LootValue%``` replacement pattern to ```%ShipLootValue%```
- Changed the ```%DaysLeft%``` replacement pattern to ```%Deadline%```
- Changed the different line names from [Ship, Quota, Days left] to [Line #1, Line #2, Line #3] since all data points are available to all lines.
##
- Added a slight angle to the UI elements so they better match other HUD elements
- Added width to the text field so it can be longer with out truncating
  - This is dynamic based on which lines are enabled due to scaling (when I make the custom scaling system it will be configurable)
- Added "Allow Inside" setting which works like "Allow Outside" but for inside the dungeon
- Added "Always On" mode - Decouples it from the scanner so you can always have it on screen
  - This mode will honor the "Allow Outside" setting => False means the UI will not show outside the ship or dungeon
  - This mode will honor the "Allow Inside" setting => False means the UI will not show inside the dungeon
- Added a configuration for duration timing (how long the UI shows after right clicking to scan. Range: 1 - 60 seconds. Default: 5 seconds)
- Added the ability to hide the line graphic
- Added a config for how many characters the "Short" data point variants return (Default: 3)
- Added support for Rich Text formatting. The line color in the config is applied to the line element first, meaning rich text formatting is applied after
  - Refer to [This Guide](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html) on how to use rich text formatting
- Added the following data points:
  ```cfg
   %MoonLootValue%      = Value of all scrap on the moon (excluding the ship)
   %AllLootValue%       = Value of all scrap (ship and moon)
   %ShipLootCount%      = Count of scrap items in the ship (excluding the moon)
   %MoonLootCount%      = Count of scrap items on the moon (excluding the ship)
   %AllLootCount%       = Count of scrap items (ship and moon)
   %CompanyRate%        = Current company buy rate
   %ExpectedProfit%     = Expected profit from scap on ship at current company buy rate
   %DeadlineWithColors% = Quota deadline in days but changes colors based on value (color thresholds will be customizable in the future)
   %DayNumber%          = Integer of days in the ship/save (E.g. 1, 3 ,10)
   %DayNumberHuman%     = Human friendly days in the ship/save (E.g. 1st, 3rd, 10th)
   %Weather%            = Current moons weather full name
   %WeatherShort%       = Current moons weather short name (Default is first 3 characters)
   %MoonLongName%       = Current moons full name
   %MoonShortName%      = Current moons short name (Default is first 3 characters)
  ```
##
- Removed the ```Position``` drop down configuration item as it is no longer needed
  - This will be replaced with a coordinate based position system in a later update

</br>

# v1.0.0
- Release