# ShootAR
This is our graduation project for [Computer & Informatics Engineering Department of Technological Educational
Institute of Western Greece](http://www.cied.teiwest.gr).

ShootAR is an AR game for Android built with the Unity game engine. Enemies appear in the area around the player
and the player uses the phone's camera and gyroscope to aim and shoot them.

*If you have a question that was not answered in this readme or the
[closed issues](https://github.com/AnimaRain/ShootAR/issues?q=is%3Aclosed+label%3Aquestion), or you find a mistake
or discrepancy in this readme, you are more than welcome to open a new issue or contact one of the [maintainers](#authors).*

## Table of contents
1. [Requirements](#requirements)
2. [Installation](#installation)
3. [Building](#building)
   1. [Required software](#required-software)
   2. [Downloading the source files](#downloading-the-source-files)
   3. [Compiling into APK](#compiling-into-apk)
4. [How to play](#how-to-play)
5. [Contributing](#contributing)
6. [Known issues](#known-issues)
7. [Changelog](#changelog)
8. [FAQ](#faq)
9. [License](#license)
10. [Authors](#authors)
11. [Credits](#credits)

## Requirements
An Android device running at least version 5.0 ("Lollipop") and it must have gyroscope.

## Installation
The easiest way to install ShootAR, is to download the `.apk` of the latest release from
[here](https://github.com/AnimaRain/ShootAR/releases) to your phone and run the installation.

*Note: By default, installation of APK files downloaded outside the Google Play is disabled.
Enable "Unknown Sources" in the phone's (Security) settings.*

If you want to built it yourself, you can follow the [instructions below](#building).

## Building
*Below are the instructions on how to build the project. For more details on developing read
"[Contributing](docs/contributing.md)".*

### Required software
In order to build the project yourself, you need to have [Unity](https://unity3d.com/) installed.

### Downloading the source files
Download the latest released source files from [here](https://github.com/AnimaRain/ShootAR). If you prefer a previous
or a test version, visit the [releases](https://github.com/AnimaRain/ShootAR/releases) page.

Alternatively, download the source using git:
```
git clone https://github.com/AnimaRain/ShootAR.git <directory> 
```
or
```
git clone git@github.com:AnimaRain/ShootAR.git <directory>
```
where `<directory>` is the path to the desired location to download the files.

### Compiling into APK
Decompress the files if downloaded the ZIP or tarball version.

Create a folder named "apk" in the root of the project. (Technically, any folder would
be fine, but we have instructed git to ignore this one.)

Open the project folder in Unity:
* go to `File` –> `Build Settings...`
* if the selected platform is not Android, select it and hit the `Switch Platform` button
* set build system to `Gradle`

* to build a **development build**:
  * enable `Development Build`
  * (recommended) set compression method to `LZ4`
	
* to build a **release build**:
  * go to `File` –> `Build Settings...`
  * press the `Player Settings...` button
  * got to the `Inspector` tab and find the `Publishing Settings`
  * if you don't have a keystore file, you can create your own:
    * choose `Create a new Keystore...`, press `Browse Keystore` and type the name of the file to be created
    * type the new keystore password twice
  * if you have a keystore file:
    * choose `Use Existing Keystore`, press `Browse Keystore` and select the file
    * enter the keystore password
  * if you don't have a key:
    * open the drop–down menu next to *Alias* and select `Create a new key...`
    * fill the form that has popped up and hit `Create Key`
  * choose your key alias
  * enter your key password
  * return to the *Build Settings* window
  * (recommended) set compression method to `LZ4HC`
	
* hit `Build`
* choose a name for the APK file and save it inside the `apk` folder

Now the APK file can be transferred to an android device and install it.

Alternatively, instead of `Build`, connect an Android device with a USB cable to the computer, go to *Developer Options*
in your phone settings, enable *USB Debugging* and set *USB Configuration* to `MTP`. In the *Build Settings* window, set
*Run Device* to your device and hit `Build and Run`. This will do the same as `Build` in addition to installing the
game to the selected device.

*Note: If the device can not be detected by Unity, try restarting Unity while the device is already
physically connected to the computer.*

## How to play
To start playing hit the *Play* button in the main menu. Enemies will begin appearing in the space around you. Move
the phone around to find them, line them up with the aim dot and tap the screen to shoot them. For every enemy killed,
you get points. Destroying the capsules floating around you, gives you more bullets. Defeat all enemies to advance to the
next level. The game is over when player runs out of bullets or health.

To start playing with a higher difficulty, go to *Select* in the main menu and choose the starting level.
(warning: see (1) in the [known issues section](#known-issues).

## Contributing
Feel free to contribute to this project. To learn how you could help, read the ["Contributing"](/docs/contributing.md) page.

You could, also, just share the game with your friends. It is and will always be free.

## Known issues
1. The game has not been optimized yet in any way. After a few levels, way too many enemies appear.
2. The video capture from the camera does not scale and ignores the screen's ratio.

[More issues.](https://github.com/AnimaRain/ShootAR/issues)

## Changelog
### Version 0.5.1
*WIP*

## ~FAQ~

## License
The project is currently under the [MIT license](/LICENSE).<br/>

The general gist translated in Human is that you can pretty much do anything you want with this project.

Third party licenses:<br/>
Unity Asset Store \[[license](https://unity3d.com/legal/as_terms)\]<br/>

If you believe that your work has been published to this repository without permission, please contact me at
ioannis_spyr@yahoo.gr.

## Authors
John Spyropoulos, a.k.a. AnimaRain \<ioannis_spyr@yahoo.gr\> | [@rainsoulwhisper](https://twitter.com/rainsoulwhisper)<br/>
Ioannis Tantaoui

## Credits
A thank–you to Petros Kouvariotis for being an awesome friend, playtester and supporter!

Unity store assets used:<br/>
Drone: https://assetstore.unity.com/packages/3d/low-poly-combat-drone-82234<br/>
Crasher: https://assetstore.unity.com/packages/3d/characters/aaron-s-assets-89273
