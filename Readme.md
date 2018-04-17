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
This is just a template of a plugin project written in VB.NET language.

You can star writting a new plugin taking this template project as a start point.

## MultiLauncher
This plugin automatically launchs your favorite files or 3rd party programs for Hearthstone at SmartBot's startup.

You can run any kind of executable type, which is not limited to .exe files.

You can even run Blizzard's client if you like. 

## PanicButton
This plugin will stop the bot or terminate SmartBot process when a specified hotkey combination is pressed.

You can stablish a hotkey combination of 1, 2 or 3 simultaneous keys.

You can literally specify any single keyboard key or override any special hotkey (like CTRL+C) from the available range of keys that I provided.

The plugin registers a new, temporary system-wide hotkey.

During the lifetime of SmartBot process and while the plugin is activated, you can press the hotkey combination anywhere on the screen.

When the plugin is deactivated or SmartBot process is terminated, the system-wide hotkey is unregistered.

Don't be worried about, Windows operating system will ensure itself that the temporary hotkey ﻿gets unregistered,
so the functionality of any modified key (or overriden operating system hotkey) will return to normal. 

## SystemTrayIcon
This plugin will display a system tray icon that adds some enhancements to bring a new user experience when using SmartBot,﻿
such as the ability to minimize the window to system-tray, restore from system-tray, terminate SmartBot process,
and also will display your current win rate statistics when hovering the mouse cursor over the system tray icon. 

## TaskBarInfo
This ﻿plugin will display progress information on SmartBot's taskbar icon﻿﻿.

For example, when you are in a arena run, it will display the current wins and losses,
this way you can keep track of your progress when SmartBot window is minimized.

A progressbar indicator is also shown in the taskbar icon. ﻿

For other game modes, it will only display your hero's class name and the enemy's class name﻿﻿.
Since the visible text capacity of a taskbar icon is very small, I decided to just display that info﻿﻿. 

## UltimateEmoter
This plugin automatically sends emotes to enemies on certain conditions defined by the user.
I added only few configurable conditions for this initial release, in the future I will add more.

The plugin has also a condition to squelch/mute the enemy. 

## WindowRestorator
This plugin will restore the last size and position of SmartBot's window the next time you run it.

It also restores the maximized state if SmartBot was maximized when you terminated its ﻿process.

It will don't restore the minimized state since I consider it useless. (who wants to run minimized a program?)

 