# ShootAR
This is our graduation project for [Computer & Informatics Engineering Department of Technological Educational Institute of Western Greece](http://www.cied.teiwest.gr).

ShootAR is an AR game for Android built with the Unity game engine. Enemies appear in the area around the player
and the player uses the phone's camera and gyroscope to aim and shoot them.

*If you have a question that was not answered in this readme or the
[closed issues](https://github.com/AnimaRain/ShootAR/issues?q=is%3Aclosed+label%3Aquestion), or you find a mistake
or discrepancy in this readme, you are more than welcome to open a new issue or contact one of the [maintainers](#authors).*

## Table of contents
1. [Requirements](#requirements)
2. [Installation](#installation)
3. [Building](#building)
4. [How to play](#how-to-play)
5. [Contributing](#contributing)
	1. [Development environment](#development-environment)
	2. [Coding style](#coding-style)
	3. [Tests](#tests)
	4. [Shipping changes](#shipping-changes)
		1. [Pull request and branch](#pull-request-and-branch)
		2. [Writing commits](#writing-commits)
		3. [Push to upstream](#push-to-upstream)
6. [Known issues](#known-issues)
7. [Changelog](#changelog)
8. [License](#license)
9. [FAQ](#faq)
10. [Authors](#authors)
11. [Credits](#credits)

## Requirements
An Android device running at least version 5.0 ("Lollipop") and it must have gyroscope.

## Installation
The easiest way to install ShootAR, is to download the `.apk` of the latest release from [here](https://github.com/AnimaRain/ShootAR/releases) to your phone and run the installation.

*Note: By default, installation of APK files downloaded outside the Google Play is disabled. Enable "Unknown Sources" in the phone's (Security) settings.*

If you want to built it yourself, you can follow the [instructions below](#building).

## Building
In order to build the project yourself, you will need to have [Unity](https://unity3d.com/) installed.
(If you meet the legal requirements, the [personal](https://store.unity.com/download?ref=personal) plan will do just fine.)

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

Create folder `apk` in the

Decompress the files and open the project folder in Unity:
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
	* if you have a keystore file:
		* choose `Use Existing Keystore`, press `Browse Keystore` and select the file
		* enter the keystore password that you have been provided
		* choose your key alias and enter your password
	* if you don't have a keystore file, you can create your own:
		* choose `Create a new Keystore...`, press `Browse Keystore` and type the name of the file to be created
		* type the new keystore password twice
		* open the drop–down menu next to *Alias* and select `Create a new key...`
		* fill the form that has popped up and hit `Create Key`
		* enter your key password
	* return to the *Build Settings* window
	* (recommended) set compression method to `LZ4HC`
	
* hit `Build`
* choose a name for the APK file and save it inside the `apk` folder

Now the APK file can be transferred to an android device and install it.

Alternatively, instead of `Build`, connect an Android device with a USB cable to the computer, go to *Developer Options*
on the phone, enable *USB Debugging* and set *USB Configuration* to `MTP`. In the *Build Settings* window, set
*Run Device* to your device and hit `Build and Run`. This will do the same as `Build` in addition to installing the
game to the selected device.

*Note: If the device can not be detected by Unity, try restarting Unity while the device is already physically connected to the computer.*

## How to play
To start playing hit the *Play* button in the main menu. Enemies will begin appearing in the space around you. Move
the phone around to find them, line them up with the aim–dot and tap the screen to shoot them. For every enemy killed,
you get points. Destroying the capsules floating around you, gives you more bullets. Defeat all enemies to advance to the
next level. The game is over when player runs out of bullets or health.

To start playing with a higher difficulty, go to *Select* in the main menu and choose the starting level. (warning,
see (1) in the [known issues section](#known-issues).

## Contributing
Feel free to contribute to this project. You could help with bug fixing; play the game and report any bug you may find ~~or
create a pull request and start fixing any known issue~~. Ideas and suggestions are also welcomed. To report an issue or
view the existing ones, go [here](https://github.com/AnimaRain/ShootAR/issues). Or you could just share the game with your
friends. It is and will always be free.

If you are not interested in developing the project, skip to the [next section](#known-issues).

### Development environment
Read the [building](#building) section.

(Note that the Unity editor runs only under Windows and Mac.)

On Windows, it is required to have a tool that can reliably utilize git's full potential, like [git for Windows](https://gitforwindows.org/).
Also, download [git-lfs](https://git-lfs.github.com/), which handles the storage of large files in the repository.

### Coding style
Please, try to follow the existing coding style. If looking at the existing code results in ambiguity, at least follow these
[coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
and [naming conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines), as described
by MSDN. The style shown in the coding conventions link can be flexible; you may notice that
some rules of the guideline are not followed but if you are not sure of what the alternative writing style is, use the one provided
in the link. On the other hand, using this naming convention is mandatory.<br/>
Mandatory rules to follow as well are the following:
* The width of indentation is 4 space characters. Tabs are preferred over spaces, but spaces are also allowed.
Spaces can be used after tabs to fine–tune the indentation.
* Do **not** start an element's name with an underscore ( \_ ).
```C#
// Instead of this...
private int _foo;

// do this. Even for private members.
private int foo;
```
* Give each element a descriptive enough name.
```C#
// Let's suppose that we need to store an object's width.
// Instead of...
int x;
// or...
int num;
// or...
int w;

// use this.
int width;

/* Depending on the situation, an even more descriptive name may be preferable.
 * Let's suppose that in the middle of a function, we need to store an image's and a box's width. */ 
private void SomeFunction(Image image, Box box){
	// ...
	float imageWidth = image.Width;
	float boxWidth = box.Width;
	// ...
}
```
* An element's name should **not** reflect its type. **Do NOT use Hungarian notation!** The only exception accepted where
the name reflects the type is, for example, `Button FireButton`, where the property's name could not have a more fitting name.
But naming an object after its class–type is OK. For example, `private GameManager gameManager;` is fine.
* Write [documentation comments](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments).
* Avoid obvious statements in comments.
```C#
// Avoid this type of comments.

// Assign the age.
int age = 1;

// Damage player
void DamagePlayer(){
	player.Health -= damage;
}

// Restore health if damaged
if (player.Health < Player.MaxHealth)
	player.RestoreHealth();
```
* If you think a piece of code is not easily comprehensible, comment it. If you can not write a comprehensible comment
either, try a different approach to the problem.
* Do **not** write/do something if you can not reason about it. Doing otherwise will result in unmaintainable bad code.
This also applies to every other aspect of the project, not only code. History can explain it better.
[This](https://github.com/AnimaRain/ShootAR/pull/23) was the result when the rules were not applied.

### Tests
Write tests if applicable. The tests should  If you prefer TDD or writing tests after or any other way, it is your own choice.
The limitations imposed by Unity's *MonoBehaviour* class make it difficult to write unit tests without making the code more
complicated than it should be. Current tests are an in–between of unit tests and implementation tests. Still write simple
and one–thing focused tests as if they were unit tests. If you have a suggestion on a better way of writing tests, please
feel free to share it.

### Shipping changes
#### Pull request and branch
Create a branch branching out of the *master* branch, where you will be storing your changes. Can be done through a GUI,
e.g. using Visual Studio or use git: (do `git checkout master` if you are not in *master*) `git checkout --branch
<branch-name>`, where *<branch-name>* is the name of your branch. Give the branch a good descriptive name. If you want,
push the branch to the server to keep a safe back–up of your work.

[Create a pull request](https://github.com/AnimaRain/ShootAR/pulls) in which, what change will be made, must be described
clearly. Alterations to the pull request description can be made along the way of working on your local branch, to better
portray the changes. The pull request is not required to be made from the beginning, but doing so, will help by allowing
others to provide feedback and save time by pinpointing discrepancies to the pull request beforehand instead of when you
think you are ready to push to the server. If you have decided to create it beforehand, please mark it "work in progress"
(WIP).

#### Writing commits
Follow the classic commit writing style: i) a brief summary of what was changed, ii) an empty line iii) followed by a more
elaborate explanation of the commit, iv) an empty line v) followed by a signature `Signed-off-by: name <email>`.
```
Fix wrong number shown in defeat message

What ever round may the game start in, the survived rounds are
always counted from 0 to the current round. The fix is subtracting
the starting level from the current level (where the game over happened).

Signed-off-by: AnimaRain <ioannis_spyr@yahoo.gr>
```
Only the brief summary is required; the rest are optional, but strongly advised to use them when it seems appropriate. When writing
a verbose commit, do not forget the empty lines. They are important.

A rule of thumb; the summary should be written so it completes the sentence "Applying this commit will...",
e.g. "Applying this commit will *Fix wrong number shown in defeat message*".

Each commit should be focused on a distinct objective.<br/>For example, the previous example was taken from
[this commit](https://github.com/AnimaRain/ShootAR/pull/23/commits/b597f7672a6b003c984da2e1c17e5ceca79396b0). This should
have been two commits; the first being the fixing of the bug and the other the code clean–up, somewhat like
the previous and the following examples:
```
Clean up code

Did some refactoring and removed a forgotten .meta file.

Signed-off-by: AnimaRain <ioannis_spyr@yahoo.gr>
```

To avoid making the same mistake as in the commit in the link, avoid automatically adding all changed files to the
commit and take a more manual and investigative approach. Use `git add --all` only when you are absolutely sure of
the changes made.

Also, if you make unrelated small fixes along the way, keep track of them; when you are done, unfix them and then
add the files to the commit, i.e. `git add --all`, and then redo the fixes. Now you can `git commit` without including
the small fixes and then `git add --all` and `git commit` again to commit the small fixes. Note that those small
fixes might be better to be split up in more than one commit, depending on the situation. You do not have to use
this exact technique as long as the result is the same.<br/>
(In the example `git add --all` is used, presuming that we are only dealing with files that we know what changes were made to.)

You are not required to squash your commits. **This project is all about learning.** This also applies when you make a commit
just to fix a typo missed in the previous commit. **It's fine! Humans make mistakes.** By keeping commits visible, one can
learn from previous mistakes, helps better understanding the intentions of the coder, and it is easier to find
the point at which a mistake was made.

 For additional rules and more detail, read this very well–written [guide](https://chris.beams.io/posts/git-commit/)
 by Chris Beams. You are also expected to follow those rules.

#### Push to upstream
Before uploading anything read the [license section](#license). Also, if you have intention of uploading anything
that has been made by a third party, make sure that you have their permission and that there is no conflicts with
the license and principles of this project.

To push the commits to the server having a branch and a pull request is required (as [described above](#pull-request-and-branch)).

After pushing the last commits to the upstream branch, `git push` or if there is no upstream branch set, `git push
--set-upstream original <branch-name>`. If you have not created a pull request yet, do so (see [above](#pull-request-and-branch)).
If you have already made one, go to your request and notify that your branch is ready to be merged. Await review by
an administrator.

## Known issues
1. The game has not been optimized yet in any way. After a few levels, way too many enemies appear.
2. The video capture from the camera does not scale and ignores the screen's ratio.

[More issues.](https://github.com/AnimaRain/ShootAR/issues)

## Change log
### Version 0.5.1
*WIP*

## License
The project is currently under the [MIT license](/LICENSE).

The general gist translated in Human is that you can pretty much do anything you want with this project.

If you believe that your work has been published to this repository without permission, please contact me at
ioannis_spyr@yahoo.gr.

## FAQ

## Authors
John Spyropoulos, a.k.a. AnimaRain \<ioannis_spyr@yahoo.gr\> | [@rainsoulwhisper](https://twitter.com/rainsoulwhisper)<br/>
Ioannis Tantaoui

## Credits
Petros Kouvariotis for being an awesome friend, playtester and supporter!

Drone: https://assetstore.unity.com/packages/3d/low-poly-combat-drone-82234
Crasher: https://assetstore.unity.com/packages/3d/characters/aaron-s-assets-89273
