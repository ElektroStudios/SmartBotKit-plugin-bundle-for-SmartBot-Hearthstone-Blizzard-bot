# **SmartBotKit**

**SmartBotKit** is a personal collection of plugins developed for **SmartBot**, a bot for **Hearthstone** videogame.

Please be aware that using bots/cheats is against **Blizzard** EULA:

http://eu.blizzard.com/en-gb/company/legal/anti-cheating.html

These plugins were developed just for fun, and are shared here only for educative purposes. 

I don't use bots, I'm just a enthusiastic programmer that like to learn new things about how are built and how functions this kind of software for video games.

# **The Plugins**

## SmartBotKit.Core
This is the main assembly required ﻿for all my plugins. So you need to install this.﻿ 

This is a public API that extends the official SB API (nothing special, just a little bit of reusable code used ﻿for my plugins).

The library exposes members to interoperate with SmartBot process and Win32 API, which are required by my plugins

Installation:

![](https://i.imgur.com/CjbUmW8.png)

## Template
This is just a plugin template written in VB.NET language.

You can star writting a new plugin for SmartBot taking this template project as a startup point.

## Auto Cleaner
This plugin will automatically clean temporary/garbage files when exiting from SmartBot. 

Settings:

![](https://i.imgur.com/ngublGM.png)

## Hearthstone Resizer
This plugin will automatically move and resize the Hearthstone window to a speciied size and location.

Just that.

You can configure the plugin to resize the window every timer tick, or each 5 ticks, for example.

The plugin is aware of when Hearthstone window is maximized, and when it is at fullscreen mode.

It will not try to move/resize its window in those circunstances.
(however, I didn't tested the fullscreen mode detection on multi-monitor configurations)

Note that I didn't provided any 16:9 resolution just because Hearthstone process does not like those resolutions;

when attempting to resize Hearthstone window to a 16:9 size, its process will automatically change to a different size.

Settings:

![](https://i.imgur.com/RajVU6i.png)

## Multi Launcher
This plugin automatically launchs your favorite files or 3rd party programs for Hearthstone at SmartBot's startup.

You can run any kind of executable file type, which is not limited to .exe files.

You can even run Blizzard's Battle.net client if you like. 

Settings:

![](https://i.imgur.com/BMZavKT.png)

## Panic Button
This plugin will stop the bot or terminate SmartBot process when a specified hotkey combination is pressed.

You can stablish a hotkey combination of 1, 2 or 3 simultaneous keys.

You can literally specify any single keyboard key or override any special hotkey (like CTRL+C) from the available range of keys that I provided.

The plugin registers a new, temporary system-wide hotkey.

During the lifetime of SmartBot process and while the plugin is activated, you can press the hotkey combination anywhere on the screen.

When the plugin is deactivated or SmartBot process is terminated, the system-wide hotkey is unregistered.

Don't be worried about, Windows operating system will ensure itself that the temporary hotkey ﻿gets unregistered,
so the functionality of any modified key (or overriden operating system hotkey) will return to normal. 

Settings:

![](https://i.imgur.com/tUXTWGv.png)

## System Tray Icon
This plugin will display a system tray icon that adds some enhancements to bring a new user experience when using SmartBot,﻿
such as the ability to minimize the window to system-tray, restore from system-tray, terminate SmartBot process,
and also will display your current win rate statistics when hovering the mouse cursor over the system tray icon. 

Preview:

![](https://i.imgur.com/Kpx6rXQ.png)

Settings:

![](https://i.imgur.com/a7NKPxC.png)

## TaskBar Info
This ﻿plugin will display progress information on SmartBot's taskbar icon﻿﻿.

For example, when you are in a arena run, it will display the current wins and losses,
this way you can keep track of your progress when SmartBot window is minimized.

A progressbar indicator is also shown in the taskbar icon. ﻿

For other game modes, it will only display your hero's class name and the enemy's class name﻿﻿.
Since the visible text capacity of a taskbar icon is very small, I decided to just display that info. 

Preview:

![](https://i.imgur.com/4kU0sbu.png)

Settings:

![](https://i.imgur.com/eKiY7fT.png)

## Ultimate Emoter
This plugin automatically sends emotes to enemies on certain conditions defined by the user.
I added only few configurable conditions for this initial release, in the future I will add more.

The plugin has also a condition to squelch/mute the enemy. 

Settings:

![](https://i.imgur.com/mL8zInj.png)

## Window Restorator
This plugin will restore the last size and position of SmartBot's window the next time you run it.

It also restores the maximized state if SmartBot was maximized when you terminated its ﻿process.

It will don't restore the minimized state since I consider it useless. (who wants to run minimized a program?)

Settings:

![](https://i.imgur.com/EGyBLgA.png)
