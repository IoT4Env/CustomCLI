# CustomCLI

Personally i'm a fan of CLI, so i decided to develop one of them from SCRATCH.
By that i mean no external libraries or tools, just pure C# code that i know what it does and how to write it.

## How does it work

This CLI simulates the cmd from Windows and Linux the one that let the user navigate in the file system (fs).
So it can create files, folders, print stuff on the terminal, etc...
Commands used in this project are a mix of both Windows and Linux, although some command names are different.

## A virtual environment to play with

The name CustomCLI might make the user think that it activly interact with the real fs of the real PC, but it does not.
All actions made inside this programm are objects whose existence is binded to the program itself.
In other words, anything "created" with CustomCLI is NOT created in the real user PC!

## User guide

On startup, a terminal appears with a breaf description of the author, the purpose, and what commands are currently available.
Some description is provided to those commands, however a better explaination can be read downwards:

- help

Syntax:\
help

displays current commands and their description

- cls

Syntax:\
cls

Clears the terminal of all the printed strings.

- exit

Syntax:\
exit

Terminates the process.

- echo

Syntax:\
echo hello\
echo "hello world!"

Prints text defined after the echo keyword.\
Additional world can be printed simultaniusly, but must be delited by the quotation marks (").

- cd

Syntax:\
cd folder/

Moves the user up from the current folder to another one if present.\
It can be stacked with multiple folders, but bare in mind that autocompletion is not present.

-fd

Syntax:\
fd number

Mmover the user down from the current folder to a number of directories specified after the fd keyword.

- touch

Syntax:\
touch file.txt\
touch folder/file.txt

Creates a file in the current folder.\
It is also possible to create a file in a different folder.

- rm

Syntax:\
rm file.txt

Removes a file in the current folder.

- mkdir

Syntax:\
mkdir folder

Creates a new folder inside the current folder

- rmdir

Syntax:\
rmdir folder

Removes the specified folder.\
If that folder is not empy, it has to be cleaned of all it's content manually before deletion.

- ls

Syntax:\
ls

Lists all elements inside the current folder

- edit

Syntax:\
edit file.txt

Lets the user edit the specified file.\
The edit functionality is currently not event close to a real editor, just the base concept of editing a file.

- cat

Syntax:\
cat file.txt

Prints the content of the specified file.

- mv

Syntax:\
mv file.txt->folder/

The mv command must be written as a contiguous set of characters without spaces in between.\
The resulting string is splitted by the string "->", thus permitting to handle the argument as two indipendent strings.
