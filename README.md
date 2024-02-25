# ShipLoot Plus (+)
Rewrite and enhancement to the original [ShipLoot](https://thunderstore.io/c/lethal-company/p/tinyhoot/ShipLoot/) with a ton customization.

This started as a fork but I ended up rewriting 99% of it to allow for more dynamic customization.

> **_IMPORTANT_**: With **v1.0.1** there has been some changes which will reset any custom line formats and colors. You do not need to regenerate your configs and these settings will still be in your config file, its just that the line names and categories have changed (though now you would have extra data in the files). Please review the [Change Log](https://thunderstore.io/c/lethal-company/p/PXC/ShipLootPlus/changelog/) for more information on what has changed as you may want to have new customizations anyway.

## Key Features
This mod adds three customizable HUD elements which allow you to display many different data points. This is expanded from the original ShipLoot which _only_ showed the value of scrap in the ship.

- Show more game information easily
- Highly customizable
  - Font and Display Layout
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
    %DayNumber%          = Integer of days in the ship/save (E.g. 1, 3 ,10)
    %DayNumberHuman%     = Human friendly days in the ship/save (E.g. 1st, 3rd, 10th)
    %Weather%            = Current moons weather full name
    %MoonLongName%       = Current moons full name
  ```

  ### Example Breakdown
  Default Lines:

  ```
  Ship: $%ShipLootValue%(%ShipLootCount%) / $%MoonLootValue%(%MoonLootCount%) <i>[%MoonName%:%Weather%]</i>
  Quota: $%FulfilledValue% / $%QuotaValue% - Profit: $%ExpectedProfit%(%CompanyRate%%)
  Deadline: %Deadline% - %DayNumberHuman% day
  ```

  Translated Lines:

  ```
  Ship: $209(2) / $0(0) [Offense:Clear]
  Quota: $0 / $229 - Profit: $63(30%)
  Deadline: 3 - 5th day
  ```

  ### DataPoint Shortening
  Each data point can be shortened so that it shows a truncated version of the data. All you have to do is add a ':' and a number and it will truncate that value to the number provided

  - ```%MoonName:3%``` becomes ```Off```
  - ```%MoonName:5%``` becomes ```Offen```

  ### Rich Text Support
  You can introduce further customizations inline with your formats.

  Here are some resources to understand Rich Text and what tags are available (not every tag will work as the font that the game uses lacks some support)
  - [Style text with rich text tags](https://docs.unity3d.com/Manual/UIE-rich-text-tags.html)
  - [Supported rich text tags](https://docs.unity3d.com/Manual/UIE-supported-tags.html)

  Inline Rich Text formatting will override the text color for the specific word or DataPoint in the line, they are applied after the text line color configuration.

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Color-RichText.png?raw=true)

  ### Specific DataPoint settings
  Some data points have specific settings tied to them

  <details>
    <summary>Deadline</summary>

  Customizing how the ```%Deadline%``` data point is displayed
  
  ### Zero Replacement
  By default when the deadline reaches zero, it is replaced with "**NOW!**", this can be disabled or the word can be changed in the config

  ### Color Coding
  You can enable/disable color coding directly for this data point in the config, it is disabled by default.

  The color is set using the following thresholds:
  - 2+ Days left
  - 1 Day left
  - 0 Days left

  Each color can be customized in the config

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-2Plus.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-1.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-ColorDeadline-0.png?raw=true)

  ### Configuration

  ```cfg
    [DataPoint: Deadline]

    ## Enables color for the deadline number
    # Setting type: Boolean
    # Default value: false
    Use Colors = false

    ## Replace the number 0 with the custom text below, otherwise leave it as a number
    # Setting type: Boolean
    # Default value: true
    Replace Zero = true

    ## Text to replace the number Zero if 'Replace Zero' is enabled
    # Setting type: String
    # Default value: <b>NOW!</b>
    Zero Replacement = <b>NOW!</b>

    ## Color for when the deadline has two or more days remaining
    # Setting type: String
    # Default value: 00FF00
    Color: 2+ days = 00FF00

    ## Color for when the deadline has one day remaining
    # Setting type: String
    # Default value: FFA500
    Color: 1 day = FFA500

    ## Color for when the deadline is due
    # Setting type: String
    # Default value: FF0000
    Color: Zero days = FF0000
  ```

  ---

  </details>

  <details>
    <summary>Weather</summary>

  Customizing how the ```%Weather%``` data point is displayed

  ### None/Clear Weather Text
  By default when there is no weather, instead of showing "None" its changed to "Clear". This can be changed in the config or set to nothing/blank to show "None"

  ### Color Coding
  You can enable/disable color coding directly for this data point in the config, it is disabled by default.

  It will change the color of the weather based on what weather it is, the colors can be customized in the config

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Clear.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Foggy.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-DustClouds.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Rainy.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Stormy.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Flooded.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Eclipsed.png?raw=true)
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-Weather-Hell.png?raw=true)
  > **NOTE**: The "Hell" weather type is added by the [HellWeather](https://thunderstore.io/c/lethal-company/p/stormytuna/HellWeather/) mod

  ### Configuration

  ```cfg
    [DataPoint: Weather]

    ## Text to use instead of 'None' for when the weather is clear (set to blank if you want it to show None)
    # Setting type: String
    # Default value: Clear
    Clear Weather Text = Clear

    ## Enables color for each weather type
    # Setting type: Boolean
    # Default value: false
    Use Colors = false

    ## Color for Clear/None weather
    # Setting type: String
    # Default value: 69FF6B
    Color: Clear/None = 69FF6B

    ## Color for DustClouds weather
    # Setting type: String
    # Default value: B56C4C
    Color: DustClouds = B56C4C

    ## Color for Rainy weather
    # Setting type: String
    # Default value: FFFF00
    Color: Rainy = FFFF00

    ## Color for Stormy weather
    # Setting type: String
    # Default value: FF7700
    Color: Stormy = FF7700

    ## Color for Foggy weather
    # Setting type: String
    # Default value: 666666
    Color: Foggy = 666666

    ## Color for Flooded weather
    # Setting type: String
    # Default value: FF0000
    Color: Flooded = FF0000

    ## Color for Eclipsed weather
    # Setting type: String
    # Default value: BA0B0B
    Color: Eclipsed = BA0B0B

    ## Color for Hell weather (from the mod 'HellWeather')
    # Setting type: String
    # Default value: AA0000
    Color: Hell = AA0000
  ```

  ---

  </details>

  <details>
    <summary>MoonName</summary>

  You can customize some of the ways the moon name is displayed

  ### Show the full moon name
  By default the leading numbers are removed from the moon name ```21 Offense``` becomes ```Offense```, you can disable this to show the full moon name in the config

  ### Replace Company Moon Name
  By default when you navigate/land at the company building, the moon name (Gordion) is replaced with "Company Building", you can disable this or customize what it replaces it with in the config

  ### Configuration

  ```cfg
    [DataPoint: MoonName]

    ## Show the full moon name (do not remove any leading numbers)
    # Setting type: Boolean
    # Default value: false
    Show Full Name = false

    ## Replace the name used for the company moon
    # Setting type: Boolean
    # Default value: true
    Replace Company Name = true

    ## Text to replace 'Gordion' if 'Replace Company Name' is enabled
    # Setting type: String
    # Default value: Company Building
    Company Name Replacement = Company Building

  ```

  ---

  </details>

  ### Configuration

  <details>
    <summary>Text Line Format Configuration</summary>

  ```cfg
    [Line #1]
    ## Line #1 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Ship: $%ShipLootValue%(%ShipLootCount%) / $%MoonLootValue%(%MoonLootCount%) <i>[%MoonName%:%Weather%]</i>
    Format = Ship: $%ShipLootValue%(%ShipLootCount%) / $%MoonLootValue%(%MoonLootCount%) <i>[%MoonName%:%Weather%]</i>

    [Line #2]
    ## Line #2 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Quota: $%FulfilledValue% / $%QuotaValue% - Profit: $%ExpectedProfit%(%CompanyRate%%)
    Format = Quota: $%FulfilledValue% / $%QuotaValue% - Profit: $%ExpectedProfit%(%CompanyRate%%)

    [Line #3]
    ## Line #3 text format.
    ## [Lists each DataPoint but omitted here for space reasons]
    # Setting type: String
    # Default value: Deadline: %Deadline% - %DayNumberHuman% day
    Format = Deadline: %Deadline% - %DayNumberHuman% day
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

---

</details>

<details>
  <summary>GUI Layout</summary>
  
  The layout of the HUD elements can be customized in a few ways.

  <details>
  <summary>Font Settings</summary>
    
  You can customize how the font is displayed with the below configurations

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-LethalConfig-FontSettings.png?raw=true)

  #### [_Font Selection_]
  You can change which font is being used by ShipLootPlus, the default is the vanilla in-game font, but if you wanted to have dollar signs or other special characters display correctly you can.

  Font List:
  - Vanilla (Default)
  - Fixed (Regular version of the 3270Font which fixes the dollar sign)
  - FixedSemiCondensed (Same as Fixed but slightly more condensed)
  - FixedCondense (Same as Fixed but more condensed )

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-LethalConfig-FontList.png?raw=true)

  ##### Fixed
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Font-Fixed.png?raw=true)
  ##### FixedSemiCondensed
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Font-Fixed-SemiCondensed.png?raw=true)
  ##### FixedCondense
  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Font-Fixed-Condensed.png?raw=true)

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Font to use for the UI elements
    # Setting type: FontList
    # Default value: Vanilla
    # Acceptable values: Vanilla, Fixed, FixedSemiCondensed, FixedCondensed
    Font = Fixed
  ```

  </details>

  #### [_All Caps_]
  Enable/Disable all text being capitalized.

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-DefaultLayout-Caps.png?raw=true)

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Should text be in all caps?
    # Setting type: Boolean
    # Default value: false
    All Caps = false
  ```

  </details>

  #### [_Size_]
  Change the size of the font

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Adjust the font size
    # Setting type: Single
    # Default value: 19
    Size = 19
  ```

  </details>

  #### [_Character Spacing_]
  Change the Character Spacing of the font

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Adjust the spacing between characters
    # Setting type: Single
    # Default value: -6
    Character Spacing = -6
  ```

  </details>

  #### [_Word Spacing_]
  Change the Word Spacing of the font

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Adjust the spacing between words
    # Setting type: Single
    # Default value: -20
    Word Spacing = -20
  ```

  </details>

  #### [_Text Alignment_]
  Change the Text Alignment of the text fields

  > **_IMPORTANT_**: This may produce unwanted results, everything is designed to be TopLeft and changing it may cause things to not display correctly

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Change the default text alignment for all elements
    ## 
    ## <b>**IMPORTANT**</b> The elements are built to stay Top Left aligned, changing this may produce unwanted outcomes
    # Setting type: TextAlignmentOptions
    # Default value: TopLeft
    # Acceptable values: TopLeft, Top, TopRight, TopJustified, TopFlush, TopGeoAligned, Left, Center, Right, Justified, Flush, CenterGeoAligned, BottomLeft, Bottom, BottomRight, BottomJustified, BottomFlush, BottomGeoAligned, BaselineLeft, Baseline, BaselineRight, BaselineJustified, BaselineFlush, BaselineGeoAligned, MidlineLeft, Midline, MidlineRight, MidlineJustified, MidlineFlush, MidlineGeoAligned, CaplineLeft, Capline, CaplineRight, CaplineJustified, CaplineFlush, CaplineGeoAligned, Converted
    Text Alignment = TopLeft
  ```

  </details>

  #### [_Transparency_]
  Change the Transparency/Alpha of the text

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## Make the text elements more or less transparent
    # Setting type: Single
    # Default value: 0.95
    Transparency = 0.95
  ```

  </details>

  ---

  </details>

  <details>
  <summary>Layout Settings</summary>
    
  The whole layout of the GUI can be customized

  ![](https://github.com/ProfX66/ShipLootPlus/blob/main/Assets/SLP-LethalConfig-Layout.png?raw=true)

  #### [_Position_]
  You can move the location of the GUI to any place on the screen

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## The X position of the UI element group
    # Setting type: Single
    # Default value: 115
    Position: X (Left/Right) = 115

    ## The Y position of the UI element group
    # Setting type: Single
    # Default value: -169
    Position: Y (Up/Down) = -169
  ```
  
  </details>

  #### [_Scale_]
  You can change the scaling of the GUI to make it larger or smaller

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## The X scale of the UI element group
    # Setting type: Single
    # Default value: 0.6
    Scale: X (Left/Right) = 0.6

    ## The Y scale of the UI element group
    # Setting type: Single
    # Default value: 0.6
    Scale: Y (Up/Down) = 0.6
  ```

  </details>

  #### [_Rotation_]
  You can change the Z rotation of the GUI to tilt it any degree you want

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## This changes how much the UI element group is rotated on the screen
    # Setting type: Single
    # Default value: 356
    Rotation: Z (Tilt) = 356
  ```

  </details>

  #### [_Text Field Width Offset_]
  You can change how many characters are displayed before it truncates, this can be helpful if you are changing the scaling.

  This isn't measured in character count, only the width of the text field which is dynamically chosen based on how many lines are enabled.

  The offset is just added to the width, so if you want to show more you would make it a positive value, less would be a negative value

  <details>
    <summary>Configuration</summary>

  ```cfg
    ## This value allows you to offset the text field width if you want to show more or less characters on screen
    # Setting type: Single
    # Default value: 0
    Text Field Width Offset = 0
  ```

  </details>

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

  </details>

  ---

</details>

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
    - [ ] Total Value of scrap in player inventory
    - [ ] Total Value of scrap in crew inventories
    - [ ] scrap value to meet quota
    - [_**x**_] Company buying percentage
    - [_**x**_] Weather
    - [ ] "InsideLoot" value/count (Loot only inside the dungeon)
    - [ ] "OutsideLoot" value/count (Loot only not inside the dungeon and not in the ship)
    - [ ] Current time
    - [ ] Countdown until midnight
    - [ ] Available money
    - [ ] Probably more...
  - [_**x**_] Refactor the code to be more dynamic and reliable
  - [_**x**_] Optimize scrap calculation methods for performance
  - [_**x**_] Option to disable the bounding line entirely
  - [_**x**_] Option to make the HUD elements not tied to scanning so they are always shown
    - [_**x**_] Make this honor the allow outside setting, if not allowed outside then only always show while on the ship
  - [_**x**_] Enable Rich Text support for each text field (override text formatting and color for specific elements instead of just the whole line)
  - [_**x**_] Option to change the HUD elements position (so it could be moved anywhere on screen)
    - ~~[ ] Once this is working, remove the original ShipLoot incompatibility (so both could be ran together if desired)~~
  - [_**x**_] Option to change the HUD elements size/scaling (so it can be resized as well)
  - [_**x**_] Option to customize the color coded deadline colors and thresholds
  - [_**x**_] Option to customize the color coded weather colors
  - [_**x**_] Option to change font
  - [_**x**_] Option to change text settings
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
  - ~~Add the ability to send data updates to a configurable websocket endpoint via serialized json (would be disabled by default)~~
    - This is now handled by another mod [LethalLEDSign](https://thunderstore.io/c/lethal-company/p/ShakePrint/LethalLEDSign/) by ShakePrint
  
</details>

## Dependencies
> **_NOTE:_** With **v1.0.1+** LethalConfig is a soft dependency but to get the best experience it is still required by the Thunderstore package. It allows you to change any setting for ShipLootPlus at any time, even landed on a moon, and it will update everything in real time. If do not want LethalConfig in your pack, you can safely remove it and this mod will still work just fine.

- [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/): Used to configure the mod while in-game with a friendly user interface
  - **Soft Dependency** => I have made it a soft dependency so it can be removed if desired with out breaking ShipLootPlus

## Compatibility
The following are mods either tested to be compatible or not

### Incompatible
- [ShipLoot](https://thunderstore.io/c/lethal-company/p/tinyhoot/ShipLoot/): Since this is basically a full rewrite of ShipLoot, it is not compatible with it. This mod will disable the original ShipLoot if it is found.
- Likely other mods that add similar HUD elements (at least until I add the custom position and scaling features)

### Compatible (Tested by me or the community)
- [EnhancedSpectator](https://thunderstore.io/c/lethal-company/p/PXC/EnhancedSpectator/)
- [GeneralImprovements](https://thunderstore.io/c/lethal-company/p/ShaosilGaming/GeneralImprovements/)
- [AdvancedCompany](https://thunderstore.io/c/lethal-company/p/PotatoePet/AdvancedCompany/)
- [LCBetterClock](https://thunderstore.io/c/lethal-company/p/BlueAmulet/LCBetterClock/)
- [EladsHUD](https://thunderstore.io/c/lethal-company/p/EladNLG/EladsHUD//)
- Likely most mods (ill do a more comprehensive test with the most popular mods later)

## Font Asset Bundle Information
The font asset bundle I created for this mod can be used by any other mod developer if they want to. I've complied with the license by embedding it into the asset bundle as well as delivering it with the bundle itself.

Since I do not own the fonts and as per their license the fonts can be distributed with their license, I am making the asset bundle available to anyone who wants to use the updated versions of the three [_3270Fonts_](https://github.com/rbanffy/3270font) in their mod.

The fonts included are:
- 3270 Regular
- 3270 SemiCondensed
- 3270 Condensed

Download the font asset bundle here: https://github.com/ProfX66/ShipLootPlus/tree/main/Assets/Fonts

> **_NOTE:_** Please also include the "3270Fonts-LICENSE.txt" file with your mod to ensure there is zero confusion about license compliance

## Other Mods
[![EnhancedSpectator](https://gcdn.thunderstore.io/live/repository/icons/PXC-EnhancedSpectator-1.0.2.png.128x128_q95.png 'EnhancedSpectator')](https://thunderstore.io/c/lethal-company/p/PXC/EnhancedSpectator/)
[![PreloadManager](https://gcdn.thunderstore.io/live/repository/icons/PXC-PreloadManager-1.0.1.png.128x128_q95.png 'PreloadManager')](https://thunderstore.io/c/lethal-company/p/PXC/PreloadManager/)
[![PrideSuits](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuits-1.0.2.png.128x128_q95.jpg 'PrideSuits')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuits/)
[![PrideSuitsAnimated](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuitsAnimated-1.0.1.png.128x128_q95.jpg 'PrideSuitsAnimated')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuitsAnimated/)
[![PrideCosmetics](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideCosmetics-1.0.2.png.128x128_q95.png 'PrideCosmetics')](https://thunderstore.io/c/lethal-company/p/PXC/PrideCosmetics/)