# ShipLoot Plus (+)
Rewrite and enhancement to the original [ShipLoot](https://thunderstore.io/c/lethal-company/p/tinyhoot/ShipLoot/) with a ton customization.

This started as a fork but I ended up rewriting 99% of it to allow for more dynamic customization.

## Features
<details>
  <summary>Adds More Information (ShipLoot, Quota, Days left)</summary>
  
  ### Expands what information is available on scan
  ![](https://i.imgur.com/q26NBLZ.png)
  
</details>

<details>
  <summary>Fully Customizable</summary>
  
  ### Make all text capitalized
  ![](https://i.imgur.com/EndPYNC.png)
  
  ### Show only two sets of data
  ![](https://i.imgur.com/wwjqoVV.png)
  
  ### Or just have a single set
  ![](https://i.imgur.com/pkXxopx.png)
  
  ### Change colors!
  Each element color is fully customizable using color HEX codes.
  
  ![](https://i.imgur.com/rR4jOCq.png)
  
  ### Replacement Strings
  The following patterns are replaced with the corresponding information

  ```cfg
    %LootValue%      = Value of all scrap on the ship
    %FulfilledValue% = Value of turned in scrap for quota
    %QuotaValue%     = Value of current quota
    %DaysLeft%       = Days left until quota is due
  ```
  #### This allows for even further customization
  This one has different string formats showing the same data, also the position is customizable
  ![](https://i.imgur.com/2CocX6Y.png)
  
  This one shows that you can consolidate the data if you want to.
  ![](https://i.imgur.com/hvxhOp6.png)
  
</details>

## Planned Features
- Expand what data is able to be set in each line
- ~~Option to disable the bounding line entirely~~
- ~~Option to make the HUD elements not tied to scanning so they are always shown~~
- Option to change the HUD elements position (so it could be moved anywhere on screen)
- ~~Configurable timeout so you can customize how long it shows~~

## Compatibility
- Since this is basically a full rewrite of ShipLoot, it is not compatible with it. This mod will disable itself with a warning in the console stating as such if its found.

## Configuration
Everything is configurable via the config file or the [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/) menu (both the main menu and the pause menu).

<details>
  <summary>General Settings</summary>
  
  ### Allow scan hud data to be accessible outside the ship
  The ```Allow Outside``` setting lets you enable the scan information with out needing to be on the ship

  ```cfg
    ## Should the scanner hud be shown when scanning outside the ship?
    # Setting type: Boolean
    # Default value: false
    Allow Outside = false
  ```
  
  ### Force capitalization
  The ```All Caps``` setting forces all text to be capitalized.

  ```cfg
    ## Should text be in all caps?
    # Setting type: Boolean
    # Default value: false
    All Caps = false
  ```
  
  ### Change the bounding line color
  The ```Line Color``` setting allows you to change the color of the line. You can include the ```#``` symbol if you want to, the field is smart enough to know what to use correctly.

  ```cfg
    ## Line color (hex code)
    # Setting type: String
    # Default value: 2D5122
    Line Color = 2D5122
  ```
  
</details>

<details>
  <summary>Ship Loot</summary>
  
  ### Enabling or Disabling the "ShipLoot" line
  The ```Show``` setting lets you enable or disable the "ShipLoot" line entirely.

  ```cfg
    ## Shows the Ship Loot on the scan hud
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```
  
  ### Change which position the "ShipLoot" line uses
  The ```Position``` setting lets you customize which line this information is shown on

  ```cfg
    ## This is where the Ship Loot information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)
    # Setting type: ElementLocation
    # Default value: Top
    # Acceptable values: Top, Middle, Bottom
    Position = Top
  ```
  
  ### Change the "ShipLoot" text color
  The ```Color``` setting allows you to change the text color for the "ShipLoot" information. You can include the ```#``` symbol if you want to, the field is smart enough to know what to use correctly.

  ```cfg
    ## Ship Loot text color (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```
  
  ### Change the format of the "ShipLoot" text
  The ```Format``` setting lets you customize the exact format for the "ShipLoot" information.

  ```cfg
    ## Ship Loot text format (%LootValue% = Loot value on the ship)
    # Setting type: String
    # Default value: Ship: %LootValue%
    Format = Ship: %LootValue%
  ```
</details>

<details>
  <summary>Quota</summary>
  
  ### Enabling or Disabling the "ShipLoot" line
  The ```Show``` setting lets you enable or disable the "Quota" line entirely.

  ```cfg
    ## Shows the Quota on the scan hud
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```
  
  ### Change which position the "Quota" line uses
  The ```Position``` setting lets you customize which line this information is shown on

  ```cfg
    ## This is where the Quota information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)
    # Setting type: ElementLocation
    # Default value: Middle
    # Acceptable values: Top, Middle, Bottom
    Position = Middle
  ```
  
  ### Change the "Quota" text color
  The ```Color``` setting allows you to change the text color for the "Quota" information. You can include the ```#``` symbol if you want to, the field is smart enough to know what to use correctly.

  ```cfg
    ## Quota text color (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```
  
  ### Change the format of the "Quota" text
  The ```Format``` setting lets you customize the exact format for the "Quota" information.

  ```cfg
    ## Quota text format (%FulfilledValue% = Value turned in for the quota. %QuotaValue% = Full value of the current quota)
    # Setting type: String
    # Default value: Quota: %FulfilledValue%/%QuotaValue%
    Format = Quota: %FulfilledValue%/%QuotaValue%
  ```
</details>

<details>
  <summary>Days Left</summary>
  
  ### Enabling or Disabling the "Days Left" line
  The ```Show``` setting lets you enable or disable the "Days Left" line entirely.

  ```cfg
    ## Shows the Days Left on the scan hud
    # Setting type: Boolean
    # Default value: true
    Show = true
  ```
  
  ### Change which position the "Days Left" line uses
  The ```Position``` setting lets you customize which line this information is shown on

  ```cfg
    ## This is where the Days Left information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)
    # Setting type: ElementLocation
    # Default value: Bottom
    # Acceptable values: Top, Middle, Bottom
    Position = Bottom
  ```
  
  ### Change the "Days Left" text color
  The ```Color``` setting allows you to change the text color for the "Days Left" information. You can include the ```#``` symbol if you want to, the field is smart enough to know what to use correctly.

  ```cfg
    ## Days Left text color (hex code)
    # Setting type: String
    # Default value: 19D56C
    Color = 19D56C
  ```
  
  ### Change the format of the "Days Left" text
  The ```Format``` setting lets you customize the exact format for the "Days Left" information.

  ```cfg
    ## Days Left text format (%DaysLeft% = Days left to turn in quota)
    # Setting type: String
    # Default value: Days Left: %DaysLeft%
    Format = Days Left: %DaysLeft%
  ```
</details>

## Other Mods
[![PrideSuits](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuits-1.0.2.png.128x128_q95.jpg 'PrideSuits')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuits/)
[![PrideSuitsAnimated](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuitsAnimated-1.0.1.png.128x128_q95.jpg 'PrideSuitsAnimated')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuitsAnimated/)
[![PrideCosmetics](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideCosmetics-1.0.2.png.128x128_q95.png 'PrideCosmetics')](https://thunderstore.io/c/lethal-company/p/PXC/PrideCosmetics/)
[![EnhancedSpectator](https://gcdn.thunderstore.io/live/repository/icons/PXC-EnhancedSpectator-1.0.2.png.128x128_q95.png 'EnhancedSpectator')](https://thunderstore.io/c/lethal-company/p/PXC/EnhancedSpectator/)