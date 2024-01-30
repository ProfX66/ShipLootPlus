# ShipLoot Plus (+)
Rewrite and enhancement to the original [ShipLoot](https://thunderstore.io/c/lethal-company/p/tinyhoot/ShipLoot/) with a ton customization.

This started as a fork but I ended up rewriting 99% of it to allow for more dynamic customization.

> **_IMPORTANT_**: With **v1.0.1** there has been some changes which will reset any custom line formats and colors. You do not need to regenerate your configs and these settings will still be in your config file, its just that the line names and categories have changed (though now you would have extra data in the files). Please review the [Change Log](https://thunderstore.io/c/lethal-company/p/PXC/ShipLootPlus/changelog/) for more information on what has changed as you may want to have new customizations anyway.

## Key Features
This mod adds three customizable HUD elements which allow you to display many different data points. This is expanded from the original ShipLoot which _only_ showed the value of scrap in the ship.

- Show more game information easily
- Highly customizable
  - Colors (including Rich Text support)
  - Information format
- Conditional display
  - Default (on scan only)
  - Always Shown
  - Allow it to be shown when outside the ship and/or inside the dungeon

This is what the HUD looks like with its default data points.

![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout.png?raw=true)
> **_NOTE_**: You can see what the different parts mean in the breakdown below

## Feature Breakdown

<details>
  <summary>DataPoint patterns and information</summary>
  
  Below are the full list of what I call "DataPoints", essentially these DataPoints are basically replaced in the format string with the corresponding information from the game

  ```cfg
    %ShipLootValue%      = Value of all scrap on the ship
    %MoonLootValue%      = Value of all scrap on the moon (excluding the ship)
    %AllLootValue%       = Value of all scrap (ship and moon)
    %ShipLootCount%      = Count of scrap items in the ship (excluding the moon)
    %MoonLootCount%      = Count of scrap items on the moon (excluding the ship)
    %AllLootCount%       = Count of scrap items (ship and moon)
    %FulfilledValue%     = Value of turned in scrap for quota
    %QuotaValue%         = Value of current quota
    %CompanyRate%        = Current company buy rate
    %ExpectedProfit%     = Expected profit from scap on ship at current company buy rate
    %Deadline%           = Days until quota is due
    %DeadlineWithColors% = Quota deadline in days but changes colors based on value (color thresholds will be customizable in the future)
    %DayNumber%          = Integer of days in the ship/save (E.g. 1, 3 ,10)
    %DayNumberHuman%     = Human friendly days in the ship/save (E.g. 1st, 3rd, 10th)
    %Weather%            = Current moons weather full name
    %WeatherShort%       = Current moons weather short name (Default is first 3 characters)
    %MoonLongName%       = Current moons full name
    %MoonShortName%      = Current moons short name (Default is first 3 characters)
  ```

  ### Example Breakdown
  Default Lines:

  ```
  Ship: $%ShipLootValue%(%ShipLootCount%)/$%MoonLootValue%(%MoonLootCount%) [%MoonShortName%:%Weather%]
  Quota: $%FulfilledValue%/$%QuotaValue% - Prof: $%ExpectedProfit%(%CompanyRate%%)
  Deadline: %Deadline% - %DayNumberHuman% day
  ```

  Translated Lines:

  ```
  Ship: $209(2)/$0(0) [Com:Clear]
  Quota: $0/$229 - Prof: $63(30%)
  Deadline: 3 - 5th day
  ```

  ### Rich Text Support
  You can introduce further customizations inline with your formats.

  Here are some resources to understand Rich Text and what tags are available (not every tag will work as the font that the game uses lacks some support)
  - [Style text with rich text tags](https://docs.unity3d.com/Manual/UIE-rich-text-tags.html)
  - [Supported rich text tags](https://docs.unity3d.com/Manual/UIE-supported-tags.html)

  Inline Rich Text formatting will override the text color for the specific word or DataPoint in the line, they are applied after the text line color configuration.

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Color-RichText.png?raw=true)

  The ```%DeadlineWithColors%``` DataPoint uses this internally to change the color of the deadline day number from Green (2+ days) => Orange (1 day) => Red (0 days)

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-2Plus.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-1.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-0.png?raw=true)

  ### Configuration

  <details>
    <summary>Text Line Format Configuration</summary>

  ```cfg
    [Line #1]
    ## Line #1 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Ship: $%ShipLootValue%(%ShipLootCount%)/$%MoonLootValue%(%MoonLootCount%)
    Format = Ship: $%ShipLootValue%(%ShipLootCount%)/$%MoonLootValue%(%MoonLootCount%) - $%InventoryLootValue%(%InventoryLootCount%)

    [Line #2]
    ## Line #2 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Ship: Quota: $%FulfilledValue%/$%QuotaValue% - $%ExpectedProfit%(%CompanyRate%%)
    Format = Quota: $%FulfilledValue%/$%QuotaValue% - $%ExpectedProfit%(%CompanyRate%%)

    [Line #3]
    ## Line #3 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Deadline: %Deadline% - %DayNumberHuman% day
    Format = Deadline: %DeadlineWithColors% - %DayNumberHuman% day (%Weather%)
  ```

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-LethalConfig-DataPoints.png?raw=true)
  > **NOTE**: The available data points show in the right panel in LethalConfig

  </details>

---

</details>

<details>
  <summary>Display Conditions and Settings</summary>
  
  There are several conditions which change when or how the GUI is shown.

  #### [_Always Show_]
  This setting basically makes it so that the GUI will always be shown to the user and will updated data in real time

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should the hud elements be decoupled from the scanner? (Meaning it will always be shown on screen)
    # Setting type: Boolean
    # Default value: false
    Always Show = false
  ```

  </details>

  #### [_Allow Outside_]
  Enables or Disables the GUI from being seen when Outside (not in the ship and not in the dungeon)

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should the scanner hud be shown when scanning outside the ship?
    # Setting type: Boolean
    # Default value: false
    Allow Outside = false
  ```

  </details>

  #### [_Allow Inside_]
  Enables or Disables the GUI from being seen when inside the dungeon

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should the scanner hud be shown when scanning inside the dungeon?
    # Setting type: Boolean
    # Default value: false
    Allow Inside Dungeon = false
  ```
  </details>

  ### On Scan Settings

  #### [_Display Duration_]
  Sets the timeout for the GUI when a scan is initiated

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## How long in seconds should the items stay on screen. (This is ignored if Always Show is true)
    # Setting type: Single
    # Default value: 5
    Display Duration = 5
  ```

  </details>

  #### [_Reset Duration Timer On Scan_]
  This will keep the UI timeout on scan active if the scanner is activated again.

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should the duration timer get reset if you scan?
    # Setting type: Boolean
    # Default value: false
    Reset Duration Timer On Scan = true
  ```

  </details>

  #### [_Refresh Data On Scan_]
  Forces the scanner to do a data refresh when its active. This is not really needed as every data point is updated by event triggers but can allow you to on demand updated data.

  > **WARNING**: This can cause lag and potentially crashes if you are using macros to spam the scanner and/or are using mods which do it (like [Hold Scan Button](https://thunderstore.io/c/lethal-company/p/FutureSavior/Hold_Scan_Button/))

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should a data refresh be forced when scanning?
    ## 
    ## All data is kept updated when events are triggered (player grabs an item, items get moved into the ship, etc.) so this isn't required.
    ## 
    ## <b>IMPORTANT</B>: This could cause issues with any mod that makes the scanner always on
    # Setting type: Boolean
    # Default value: false
    Refresh Data On Scan = false
  ```

  </details>

---

</details>

<details>
  <summary>GUI Breakdown and Configuration</summary>
  
  The GUI consists of 4 elements, a Line Graphic and three Text fields. Each one has some common configurations

  #### [_Line Graphic_]
  This element is used to frame the data on the screen.

  <details>
    <summary>Enable/Disable: Line Graphic</summary>

  ```cfg
    ## Shows the line element
    # Setting type: Boolean
    # Default value: true
    Show Line = true
  ```

  </details>

  <details>
    <summary>Color: Line Graphic</summary>

  > **_NOTE:_** Uses standard HTML Hexadecimal color codes (Can be with or with out the # sign)

  ```cfg
    ## Line color (hex code)
    # Setting type: String
    # Default value: 2D5122
    Line Color = 2D5122
  ```

  </details>

  #### [_All Caps_]
  Force all text to be capitalized.

  <details>
    <summary>Enable/Disable: All Caps</summary>

  ```cfg
    ## Should text be in all caps?
    # Setting type: Boolean
    # Default value: false
    All Caps = false
  ```

  </details>

  #### [_Short Character Count_]
  This is the amount of characters to return for the various "Short" data point variants

  <details>
    <summary>Short Character Count</summary>

  ```cfg
    ## How many characters to show for the following data points:
    ## 
    ## %WeatherShort%
    ## Current moons weather (short name)
    ## 
    ## %MoonShortName%
    ## Current moons short name
    ## 
    ## 
    # Setting type: Int32
    # Default value: 3
    Short Character Count = 3
  ```

  </details>

  > **NOTE**: "Experimentation" becomes "Exp"

  #### [_Text Line #1_]
  This is the first (top) text line element

  <details>
    <summary>Enable/Disable: Text Line</summary>

  ```cfg
    ## Shows Line #1 on the hud.
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```

  </details>

  <details>
    <summary>Color: Text Line</summary>

  > **_NOTE:_** Uses standard HTML Hexadecimal color codes (Can be with or with out the # sign)

  ```cfg
    ## Line #1 text color. (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```

  </details>

#### [_Text Line #2_]
  This is the second (middle) text line element

  <details>
    <summary>Enable/Disable: Text Line</summary>

  ```cfg
    ## Shows Line #2 on the hud.
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```

  </details>

  <details>
    <summary>Color: Text Line</summary>

  > **_NOTE:_** Uses standard HTML Hexadecimal color codes (Can be with or with out the # sign)

  ```cfg
    ## Line #2 text color. (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```

  </details>

  #### [_Text Line #3_]
  This is the last (bottom) text line element

  <details>
    <summary>Enable/Disable: Text Line</summary>

  ```cfg
    ## Shows Line #3 on the hud.
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```

  </details>

  <details>
    <summary>Color: Text Line</summary>

  > **_NOTE:_** Uses standard HTML Hexadecimal color codes (Can be with or with out the # sign)

  ```cfg
    ## Line #3 text color. (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```

  </details>

  #### Custom Colors Example
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Color.png?raw=true)

  #### All Caps Example
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Caps.png?raw=true)

---

</details>

<details>
  <summary>Dynamic text line scaling</summary>
  
  The default layout is three lines of custom data, but sometimes that may be too much information, so you can disable any or all text lines if you want.

  Since disabling them would normally mean a gap with whitespace, I built in dynamic scaling so that it will scale up the text lines based on how many are enabled. The only down side to this is it gives you less characters per line.

![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Scaling-2Line.png?raw=true)

> **NOTE**: If the line is too long it will be truncated with ellipses

![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Scaling-1Line.png?raw=true)

> **NOTE**: This is the closest to the original ShipLoot

  Alternatively you could just disable the Line Graphic and set the Format for the line you don't want to see to an empty string (blank) to keep the original scale.

![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-CustomLayout-BlankLine2.png?raw=true)

---

</details>

## Planned and Potential Future Features

<details>
  <summary>Planned Features</summary>

  - Expand what data is able to be set in each line
    - [_**x**_] Ship scrap count
    - [_**x**_] Moon scrap value and count
    - [_**x**_] All scrap (ship + moon) value and count
    - [_**x**_] Color coded deadline (change text color based on value, 0 = red, 1 = orange, 2+ = green)
    - [_**x**_] Number of days in the ship(save)
    - [_**x**_] Same as above but human friendly format (1st, 3rd, 10th, etc.)
    - [_**x**_] Expected profit (scrap value - quota * buy rate)
    - [_**x**_] Total Value of scrap in player inventory
    - [ ] scrap value to meet profit
    - [_**x**_] Company buying percentage
    - [_**x**_] Weather
    - [ ] Add "InsideLoot" value/count (Loot only inside the dungeon)
    - [ ] Add "OutsideLoot" value/count (Loot only not inside the dungeon and not in the ship)
    - [ ] Probably more...
  - [_**x**_] Refactor the code to be more dynamic and reliable
  - [_**x**_] Optimize scrap calculation methods for performance
  - [_**x**_] Option to disable the bounding line entirely
  - [_**x**_] Option to make the HUD elements not tied to scanning so they are always shown
    - [_**x**_] Make this honor the allow outside setting, if not allowed outside then only always show while on the ship
  - [_**x**_] Enable Rich Text support for each text field (override text formatting and color for specific elements instead of just the whole line)
  - [ ] Option to change the HUD elements position (so it could be moved anywhere on screen)
    - [ ] Once this is working, remove the original ShipLoot incompatibility (so both could be ran together if desired)
  - [ ] Option to change the HUD elements size/scaling (so it can be resized as well)
  - [ ] Option to customize the color coded deadline colors and thresholds
  - [_**x**_] Configurable timeout so you can customize how long it shows
  - [_**x**_] Expand the width of the text elements so more data can be shown before it truncates
  - [ ] Expand the amount of lines that can be enabled (from a default of 3, up to 5) with auto scaling
  - [_**x**_] Remove the "Position" drop down as its not needed since any data can be set on any line
  - [_**x**_] Rename the different line names from [Ship, Quota, Days left] => [Line1, Line2, Line3, etc]
  - [_**x**_] Make LethalConfig a soft dependency in the plugin so it can be uninstalled and ShipLootPlus still be functional

</details>

<details>
  <summary>Potential Future Features (Suggestions that I may or may not do)</summary>

  - Add an additional hud element that allows you to see the exact scrap list and which items you need to make quota (would be disabled by default)
    - Make this new HUD element position configurable
    - Make this new HUD element scale configurable
  - Add the ability to send data updates to a configurable websocket endpoint via serialized json (would be disabled by default)
  
</details>

## Dependencies
> **_NOTE:_** With **v1.0.1+** LethalConfig is a soft dependency but to get the best experience it is still required by the Thunderstore package. It allows you to change any setting for ShipLootPlus at any time, even landed on a moon, and it will update everything in real time. If do not want LethalConfig in your pack, you can safely remove it and this mod will still work just fine.

- [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/): Used to configure the mod while in-game with a friendly user interface
  - **Soft Dependency** => I have made it a soft dependency so it can be removed if desired with out breaking ShipLootPlus

## Compatibility
The following are mods either tested to be compatible or not

### Incompatible
- [ShipLoot](https://thunderstore.io/c/lethal-company/p/tinyhoot/ShipLoot/): Since this is basically a full rewrite of ShipLoot, it is not compatible with it. This mod will disable itself with a warning in the console stating as such if its found.
- Likely other mods that add similar HUD elements (at least until I add the custom position and scaling features)

### Compatible (Tested by me or the community)
- [EnhancedSpectator](https://thunderstore.io/c/lethal-company/p/PXC/EnhancedSpectator/)
- [GeneralImprovements](https://thunderstore.io/c/lethal-company/p/ShaosilGaming/GeneralImprovements/)
- [AdvancedCompany](https://thunderstore.io/c/lethal-company/p/PotatoePet/AdvancedCompany/)
- [LCBetterClock](https://thunderstore.io/c/lethal-company/p/BlueAmulet/LCBetterClock/)
- [EladsHUD](https://thunderstore.io/c/lethal-company/p/EladNLG/EladsHUD//)
- Likely most mods (ill do a more comprehensive test with the most popular mods later)

## Other Mods
[![EnhancedSpectator](https://gcdn.thunderstore.io/live/repository/icons/PXC-EnhancedSpectator-1.0.2.png.128x128_q95.png 'EnhancedSpectator')](https://thunderstore.io/c/lethal-company/p/PXC/EnhancedSpectator/)
[![PrideSuits](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuits-1.0.2.png.128x128_q95.jpg 'PrideSuits')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuits/)
[![PrideSuitsAnimated](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuitsAnimated-1.0.1.png.128x128_q95.jpg 'PrideSuitsAnimated')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuitsAnimated/)
[![PrideCosmetics](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideCosmetics-1.0.2.png.128x128_q95.png 'PrideCosmetics')](https://thunderstore.io/c/lethal-company/p/PXC/PrideCosmetics/)