# Contributing
Thank you for considering to contribute. Your help is welcomed!

*All participants to the project and their actions are governed by the [Code of Conduct](/docs/code-of-conduct.md).*

## Table of contents
1. [How to help](#how-to-help)
   1. [Submitting an issue on GitHub](#submitting-an-issue-on-github)
2. [Developing the project](#developing-the-project)
   1. [Development environment](#development-environment)
   2. [Coding style](#coding-style)
   3. [Tests](#tests)
   4. [Shipping changes](#shipping-changes)
      1. [Creating a branch](#creating-a-branch)
      2. [Writing commits](#writing-commits)
	  3. [Push to upstream](#push-to-upstream)

## How to help
If while playing the game you found a bug, you can submit a bug report to let the developers know.
(see [bellow](#submitting-an-issue-on-github))

If you have any suggestion that you think might be an improvement to the project (regarding gameplay, content, design, art, etc.),
feel free to share your idea and submit a feature request. (see [bellow](#submitting-an-issue-on-github))

Otherwise, you could actively help with the development yourself. (see [bellow](#developing-the-project))

### Submitting an issue on GitHub
Before creating a new issue, please check the [list of issues](https://github.com/AnimaRain/ShootAR/issues) and make
sure that no similar issue has already been submitted.

To create an issue, go [here](https://github.com/AnimaRain/ShootAR/issues/new/choose), choose the
appropriate template and fill in the form. Please read the comments provided, styled as
`<!-- comment -->`; do not bother deleting them as they will be hidden in the submitted form.
Please remove the placeholder text, usually styled as `*[placeholder text]*`, as well as the sections
that would be provided with no information. GitHub issues support [writing in
Markdown](https://guides.github.com/features/mastering-markdown/). GitHub allows you to preview the
issue before submitting it, by selecting the "Preview" tab.

## Developing the project
### Development environment
Make sure to also read the ["Building"](/README.md#building) section in the readme file for
[additional software requirements](/README.md#required-software) and instruction on how to
build the project.

On Windows, it is required to have a tool that can reliably utilize git's full potential, like
[git for Windows](https://gitforwindows.org/).

Also, download [git-lfs](https://git-lfs.github.com/),
which handles the storage of large files in the repository.

### Coding style
Having a uniform way of writing code throughout the whole codebase, makes reading and understanding the
code easier. [Here](/docs/coding-style.md) are the conventions that apply to this project.

### Tests
Writing tests is important. All code written should be testable, although it is not always applicable; testing the UI
for example. The tests are run using the tools provided in the Unity editor. If you prefer TDD or writing tests after
or any other way, it is your own choice. Write simple and one–thing focused tests.

The limitations imposed by Unity's *MonoBehaviour* class make it difficult to write unit tests
without making the code more complicated than it should be.

*If you have a suggestion on a better way of writing tests, please feel free to share it.*

### Shipping changes
#### Creating a branch
Create a branch branching out of the *master* branch, where you will be storing your changes.
Create the branch through a GUI, like Visual Studio or Git GUI, or use git: (do `git checkout master`
if you are not in *master*) `git checkout -b <branch-name>`, where *\<branch-name\>* is the name
of your branch. Give the branch a good descriptive name. If you want, `git push` the branch to the server.

#### Writing commits
It is very important to work in one problem at a time. This allows to document the development
of the code in a clear manner. Break up the problem at task in as many smaller parts as you see
fit! Consider those smaller parts your milestones. When a milestone is reached, commit your changes.

Read this very well–written [guide by Chris Beams](https://chris.beams.io/posts/git-commit/)
on how to write good commits.

Follow the classic style of writing a commit: i) a brief summary of what was changed, ii) an empty line iii) followed by a more
elaborate explanation of the commit, iv) an empty line v) followed by a signature `Signed-off-by: name <email>`.
```
Fix wrong number shown in defeat message

What ever round may the game start in, the survived rounds are
always counted from 0 to the current round.

Signed-off-by: AnimaRain <ioannis_spyr@yahoo.gr>
```
Only the brief summary is required; the rest are optional, but strongly advised to use them when it seems appropriate. When writing
a verbose commit, do not forget the empty lines. They are important.

A rule of thumb; the summary should be written so it completes the sentence "Applying this commit will...",
e.g. "Applying this commit will *Fix wrong number shown in defeat message*".

Each commit should be focused on a distinct objective.<br/>For example, the previous example was taken from
[this commit](https://github.com/AnimaRain/ShootAR/pull/23/commits/b597f7672a6b003c984da2e1c17e5ceca79396b0). This should
have been two commits. The first being the fixing of the bug and the other the code clean–up; somewhat like
the previous and the following examples:
```
Clean up code

Did some refactoring and removed a forgotten .meta file.

Signed-off-by: AnimaRain <ioannis_spyr@yahoo.gr>
```

To avoid making the same mistake as in the commit in the link, avoid automatically adding all changed files to the
commit and take a more manual and investigative approach. Use `git add --all` only when you are absolutely sure of
the changes made.

If you did not break up your problem and made unrelated small fixes along the way, please separate
them into smaller, cohesive commits. `git add --interactive` or `git add --patch` can help to make the
process easier; for more information on how to use the command, type `git help add`.

You are not required to squash your commits. **This project is all about learning.** This also applies when it is
too late to amend the previous commit and you have to make a commit just to fix a missed typo. **It's fine! Humans
make mistakes.** By keeping commits visible, one can learn from previous mistakes, helps better understanding the
intentions of the coder, and it is easier to find the point at which a mistake was made.

When there are changes in file *ProjectSettings/ProjectSettings.asset*, before committing, make sure
that *AndroidKeystoreName* and *AndroidKeyaliasName* are empty; if they are not, open the file in an
editor and delete their values.

#### Push to upstream
Before uploading anything, read the [license section](/README.md#license). Also, if you have intention of uploading anything
that has been made by a third party, make sure that you have their permission and that there is no conflicts with
the license and principles of this project. They should, also, be credited. Make sure that none of the files to be
uploaded contain any personal info, either yours or an other's, or any information regarding security, like passwords
or keys.

Make sure that the code is passing all tests. Then push all commits to the upstream branch, `git push`
or if there is no upstream branch set, `git push --set-upstream origin <your-branch-name>`.
[Create a pull request](https://github.com/AnimaRain/ShootAR/pulls) in which what change will be
made must be described clearly. Await review by an administrator.
