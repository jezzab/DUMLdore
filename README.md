[<img src=https://media.giphy.com/media/nQfQayikD5rX2/giphy.gif>]

Compatible with:<br>
**Aircraft:** *P4, P4 Pro, Mavic, Spark, I2*<br>
**Remote Control:** *Mavic*<br>
**Goggles:** *DJI*<br>

**DOWNLOAD THE ENTIRE ZIP FILE NOT JUST THE EXE**<br>
Make sure Assistant2 is not running<br>
Turn on the aircraft/RC and allow it to fully start up<br>
Plug in USB cable from the aircraft/RC to the PC<br>
Start DUMLdore.exe<br>

**LOAD:** Will load a firmware file to flash. Some error checking is done to make sure it is valid. It is not perfect and up to the user to make sure it is.<br>
**FLASH:** Upload the firmware file to the device and begin the upgrade/downgrade.<br>
**BACKUP:** Will make a flashable firmware backup of the currently connected device if applicable. It will save the dji_system.bin file into the current directory. It may take some time. Please be patient as the status may take a few moments to update on some of the files.<br>
<<<<<<< HEAD
**UNLOCK SPARK:** Unlock a Spark that has been grounded due to not upgrading the firmware by 1 Sept 2017<br>
=======
**UNLOCK:** Unlock a Spark that has been grounded due to not upgrading the firmware by 1 Sept 2017<br>
>>>>>>> abc7370a106cec5eb169443496860c895650ce45

**NOTE**: Make sure you have atleast 50% battery remaining before you begin<br>
**FIRMWARE FILES:** https://github.com/MAVProxyUser/dji_system.bin

Thanks to hostile, the_lord, p0v and many others for the information :)

*-jezzab*

Source code:<br>
https://www.github.com/jezzab/DUMLdore-src

### #DeejayeyeHackingClub information repos aka "The OG's" (Original Gangsters)

http://dji.retroroms.info/ - "Wiki"

https://github.com/fvantienen/dji_rev - This repository contains tools for reverse engineering DJI product firmware images.

https://github.com/Bin4ry/deejayeye-modder - APK "tweaks" for settings & "mods" for additional / altered functionality

https://github.com/hdnes/pyduml - Assistant-less firmware pushes and DUMLHacks referred to as DUMBHerring when used with "fireworks.tar" from RedHerring. DJI silently changes Assistant? great... we will just stop using it.

https://github.com/MAVProxyUser/P0VsRedHerring - RedHerring, aka "July 4th Independence Day exploit", "FTPD directory transversal 0day", etc. (Requires Assistant). We all needed a public root exploit... why not burn some 0day?

https://github.com/MAVProxyUser/dji_system.bin - Current Archive of dji_system.bin files that compose firmware updates referenced by MD5 sum. These can be used to upgrade and downgrade, and root your I2, P4, Mavic, Spark, Goggles, and Mavic RC to your hearts content. (Use with pyduml or DUMLDore)

https://github.com/MAVProxyUser/firm_cache - Extracted contents of dji_system.bin, in the future will be used to mix and match pieces of firmware for custom upgrade files. This repo was previously private... it is now open.

https://github.com/MAVProxyUser/DUMLrub - Ruby port of PyDUML, and firmware cherry picking tool. Allows rolling of custom firmware images.

https://github.com/jezzab/DUMLdore - Even windows users need some love, so DUMLDore was created to help archive, and flash dji_system.bin files on windows platforms.

https://github.com/MAVProxyUser/DJI_ftpd_aes_unscramble - DJI has modified the GPL Busybox ftpd on Mavic, Spark, & Inspire 2 to include AES scrambling of downloaded files... this tool will reverse the scrambling

https://github.com/darksimpson/jdjitools - Java DJI Tools, a collection of various tools/snippets tied in one CLI shell-like application