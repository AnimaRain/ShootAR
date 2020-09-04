# Coding style
Please, try your best to follow the existing coding style.

For everyone's convenience, an [EditorConfig](https://editorconfig.org/) file is provided along the source code.
It instructs the editor which coding conventions to apply and recommend to the coder while writing. It (probably) might not
be compatible with an IDE other than Visual Studio, though. In case you are not using Visual Studio, but want to try using it;
[many IDEs natively support EditorConfig files, but many others require a plugin.](https://editorconfig.org/#download)

## Coding style rules

### Coding Conventions:
* Avoid using `this.`, unless required or statement looks ambiguous to the reader.
* Prefer predefined types over framework types.
```C#
// Prefer this
int a;
// Over this
Int32 a;
```
* May use var in the declaration of a local variable, when the type is clear from the context.
```C#
var a = 1; // ...is clearly an int.
var b = new GameObject().AddComponent<Bullet>(); // ...is (clearly) a Bullet.
var c = "...is clearly a string.";
```
* Use auto properties when the encapsulated field is not used anywhere else.
```C#
// Prefer this.
int Blop { get; set; }
// Over this.
int blop;
int Blop {
    get { return blop; }
    set { blop = value; }
}
```
* Use a lambda expression when the function is meant to be used only once.
```C#
void DestroyEverything() {
    /* Destroy all stuff */
}

gameState.OnGameOver += DestroyEverything();

// If DestroyEverything() is not used anywhere else, prefer the following:
// Without declaring DestroyEverything()...
gameState.OnGameOver += () => { /* Destroy all stuff */ };
```
* Avoid using parenthesis to clarify the order of operations. Mathematics already have rules for operator precedence.
```C#
int a = 1 + 1 / 2; // This is the expected style.
int a = 1 + (1 / 2); // The parenthesis are not needed here.
```
* Use object object initializers when applicable.
```C#
// Instead of this:
var c = new Customer();
c.Age = 21;
// Do this:
var c = new Customer() { Age = 21 };

// Instead of this:
var a = new ArrayList();
a.Add(10);
a.Add(5);
a.Add(6);
a.Add(10);
// Do this:
var a = new ArrayList { 10, 5, 6, 10 };
```
* Explicitly declare accessors.
```C#
// Do this:
public class Blop {
    private int x;
}
// Instead of this:
class Blop {
    int x;
}
```
* Use `readonly`/`const` modifier when field is not supposed to change value.
* Do **not** use non–`readonly` `static` members.

### Formatting conventions:
* The width of indentation is 4 characters. Tabs are preferred over spaces, but spaces can be
used after tabs to fine–tune the indentation.
* Write only one statement per line.
```C#
// Instead of this
int m = "Hello"; log.WriteLine(m); DoSomethingElse();

// Do this
int m = "Hello";
log.WriteLine(m);
DoSomethingElse();
```
* More than one declarations per line is allowed.
```C#
private float MaxDistanceToSpawn, MinDistanceToSpawn;
```
* Add at least one blank line between property definitions and method definitions.
* If the statement is broken in multiple lines, the continuation lines after the first line–break must
be indented by at least two extra tabs.
* Qualified names can be broken before a dot (.), if they are too long for a single line.
```C#
var fieldWithALongName = LongNamedNamespace.ClassWithAnAlsoLongName
        .ThePropertyWeNeeded;
        
objectWithALongLongLongLongLongLongLongName.
        .TheMethodWeNeedToCall();
```
* Brackets are not required for one–line blocks.
```C#
for (a = 0; a < 10; a++)
    if (true)
        DoSomething();
```
* Single line statements are fine, as long as they are simple and not too long.
```C#
for (a = 0; a < 10; a++) DoSomething();

if (true) DoSomething();
```
* Do not use spaces directly before and after parenthesis in function declaration/call.
```C#
// Don't do this!
private void TrashFunction ( a, b );
// Do this:
private void GoodFunction(a, b);
```
* Use spaces to separate operators and operands, but not parenthesis.
```C#
// Write this:
a = 5 - (8 + 3) / 12 * Mathf.Sin(50f);
// Not this:
a=5-(8+3)/12*Mathf.Sin(50f);
```
* Use a space after comma (`,`), semicolon (`;`) and colon (`:`) if they are not the last character on the line.
* Do not use spaces before or after dots (`.`).

### Naming Conventions:
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
* Name constants in all uppercase and separate words with an underscore ( _ )
```C#
private const MAXIMUM_HEALTH = 100;
```
* Use camel case for fields, local variables and parameters.
```C#
private int thisIsAField;
```
* Use Pascal case for types and non-field members.
```C#
private int ThisIsAFunction(int thisIsAParameter) {}

public class ThisIsAClass {}

private enum ThisIsAnEnum { green, blue, flu }
```
* Interfaces must begin with "I" and be in Pascal case.
```C#
public interface ISpawnable {}
```

### Commenting Conventions:
**Comments are an important part of the code!**

* Comments must be written in proper English.
* Write [documentation comments](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments).
* Comments meant to be read by an other person, should be written in a direct and formal manner.
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
[This](https://github.com/AnimaRain/ShootAR/pull/23) was the result when there were no rules.

---
Also read [this](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions) for
more info on how to correctly write in Pascal or camel case.
