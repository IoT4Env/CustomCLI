# CustomCLI

Personally i'm a fan of CLIs, so i decided to build one of them from SCRATCH.
By that i mean no external libraries or tools, just pure C# code that i know what it does and how to write it.

## How does it work

This CLI simulates the cmd from Windows and Linux, a terminal window that lets the user navigate in the file system (fs).
So it can create and remove files and folders, print stuff on the terminal, change directories, edit files, and more.
Commands used in this project are a mix of both Windows and Linux, although some command names are different.

## A virtual environment to play with

The name CustomCLI might make the user think that it activly interact with the real fs of the real PC, but it does not.
All actions made inside this programm are objects whose existence is binded to the program itself.
In other words, anything created with CustomCLI is NOT created in the real user PC!

## And for simulating REAL folders

This program can also be used to emulate entire folders and their content.
This can be usefull for education purpose, testing backup procedures, or simply to get familiar with the CLI without the worry to mess up something important in the user PC.
For further information, refer to the [Mirror Command](#mirror)

## General functionality

To fullfill a tipical CLI behaviour, the following functionalities are implemented:
1. Create files and folders that are outside the current location.

	E.G:\
	If the user is located in ```Z:>``` (the root folder) that contains the "folder1" folder, it is possible to make actions inside that folder without changing directory.\
	An example command is as follows:

	```touch folder1/file.txt```

	The output of the previous command is the creation of the "file.txt" file inside the "folder1" folder.

2. Create multi-word files and folders
	
	E.G.\
    To create the "text file number 1" file, the syntax would be:

    ```touch  "text file number 1"```

	WARNING:\
	___Commands that work with files or folders (cd, mv, cp, etcetera) containing spaces do not work.___\
	___It is recommended to create folders and files without spaces.___

## Commands guide

On startup, a terminal appears with a breaf description of the author, the purpose, and what commands are currently available.
This section provides larger description of each of every command used in this project and the correct syntax to execute the specific command:

<div id="help"></div>

- help

Syntax:\
```z:> help```

Prints current commands name and a brief description of them.

<div id="cls"></div>

- cls

Syntax:\
```z:> cls```

Clears the terminal out of all the printed stuff.

<div id="exit"></div>

- exit

Syntax:\
```z:> exit```

Terminates the process.

<div id="echo"></div>

- echo

Syntax:\
```z:> echo hello```

```z:> echo "hello world!"```

Prints text defined after the echo keyword.\
Additional world can be printed simultaniusly, but must be delited by the quotation marks (").

<div id="cd"></div>

- cd

Syntax:\
```z:> cd folder```

```z:> cd folder1/folder2```

Moves the user up from the current folder to another one if present.\
It can be stacked with multiple folders.

<div id="fd"></div>

- fd

Syntax:\
```z:> fd number```

Move the user down from the current folder by a number of directories specified after the fd keyword.

<div id="mirror"></div>

- mirror

Syntax:\
```z:> mirror C:\Users\user1\Template```

```z:> mirror "C:\Users\user1\Template for testing"```

Recreates the entire directory structure inside this simulated environment.\
It does NOT affect real files and folders in the user PC.

<div id="read"></div>

- read

syntax:\
```z:> read name```

```Some name here```

Creates the variable called "name" and stores the value inserted afterwads (in this case, "Some name here")\
Variables are stored in a virtual Heap memory object.\
To print the value stored inside the variable, refer to its name with the $ sign:

```z:> echo $name```


<div id="expr"></div>

- expr

syntax:\
```z:> expr 3 + 5```

Evaluates the expression and prints the result on the standard output.


<div id="touch"></div>

- touch

Syntax:\
```z:> touch file.txt```

```z:> touch folder/file.txt```

Creates a file in the current folder and in a different location respectively.

<div id="rm"></div>

- rm

Syntax:\
```z:> rm file.txt```

```z:> rm folder/file.txt```

Removes a file in the current folder and in a different location respectively.

<div id="mkdir"></div>

- mkdir

Syntax:\
```z:> mkdir folder```

Creates a new folder inside the current folder

<div id="rmdir"></div>

- rmdir

Syntax:\
```z:> rmdir folder```

Removes the specified folder.\
If that folder is not empty, a prompt will ask the user for folder content deletion confirmation.

<div id="ls"></div>

- ls

Syntax:\
```z:> ls```

Lists all elements inside the current folder

<div id="exit"></div>

- edit

Syntax:\
```z:> edit file.txt```

```z:> edit folder/file.txt```

Edit a file in the current folder and in a different location respectively.

<div id="cat"></div>

- cat

Syntax:\
```z:> cat file.txt```

```z:> cat folder/file.txt```

Prints the content of a file in the current folder and in a different location respectively.

<div id="x3i"></div>

- x3i

Syntax:\
```z:> x3i script.x3i```

```z:> x3i folder/script.x3i```

Executes the content of a script file in the current folder and in a different location respectively as commands interpreted by this CLI.\
The syntax for the script to work respects the syntax of all other commands

_Estear egg_\
_For those who are curious, this command name is inspired by the Organization XIII of the Kingdom Hearts franchise._\
_Because The terminal is a SHELL and Organization XIII members are special empty SHELLs (nobodies)._

<div id="mv"></div>

- mv

Syntax:\
```z:> mv file.txt->folder```

```z:> mv folder1/folder2/file.txt->folder1/folder3```

Moves a file from one location to another.\
The mv command must be written as a contiguous set of characters without spaces in between.\
The resulting string is splitted by the string "->", thus permitting to handle the argument as two indipendent strings.

<div id="cp"></div>

- cp

Syntax:\
```z:> cp file.txt->folder```

```z:> cp folder1/folder2/file.txt->folder1/folder3```

Copies a file from one location to another.\
The cp command must be written as a contiguous set of characters without spaces in between.\
The resulting string is splitted by the string "->", thus permitting to handle the argument as two indipendent strings.
