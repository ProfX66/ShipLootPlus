# v1.0.2
- Fixed an issue where "Always ON" mode did not function correctly when "Allow Outside" and/or "Allow Inside" was enabled.
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