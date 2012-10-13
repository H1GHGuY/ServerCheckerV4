ServerChecker V4
----------------
Before modifying, distributing or using this software, please read this file.


1. Goals
--------
ServerChecker V4 is the successor to ServerChecker v3 by the same original author.
The main purpose is to operate and manage server software including but not limited
to game server software.

2. Features
-----------
Besides inheriting most of the feature set of ServerChecker v3 this release comes
with new features:
- ServerChecker V4 can take control of server software that is already 
  running before ServerChecker starts.
- ServerChecker V4 can run as a windows service.
- ServerChecker V4 has remote control capabilities.
  It also supports multiple users with different access levels.
  Remote control is both encrypted and authenticated.
- It supports different levels of trust for code.
  e.g. a ServerChecker Plugin may not have the same permissions
       on the running system as ServerChecker itself.
  ServerChecker V4 itself is supposed to require as few permissions as possible.
  Serverchecker V4 is also designed to minimize influence of one subsystem to another.

3. Known bugs in the initial source code release
------------------------------------------------
As with all software ServerChecker has some known issues:
- Remoting connections are set up quite often, rather than once per session.
  This can cause slowdowns when operating the GUI.

4. Work to be done
------------------
The initial source code release is a work in progress and cannot
be seen as a finished product:
- Keys embedded in the project are to modified.
- Licensing code may be stripped out altogether since this is rather
  incompatible with the GPL License (unless everyone is granted a
  license by default).
- The GUI was originally seen as a test application for the back-end.
  It has superseded that goal during development and as such it may
  have various weaknesses and design flaws.
- The GUI will require updates of the references to the ServerChecker V4
  client library.
- a lot of finishing, testing...

5. Licensing
------------
ServerChecker V4 is published under the GNU General Public License V2 only.
For those unfamiliar with this license this means that you have not only rights
but also obligations. Below is a condensed list of some but not all of the obligations:
- When you modify files, you must add a prominent notice that you changed the files
  including a date of modification and preferably a very brief description of your change.
- Any derived work must be published as a whole under the GPL License:
  1) Given that the SC.ClientProvider library is also covered under the GPL, using it in
     a front-end application causes the front-end to fall under the GPL License.
  2) Any plugin written for ServerChecker immediately and automatically 
     falls under the GPL License.
- When distributing this product or any derivative products, you must include a written
  offer to obtain the source code for that product.
  Derivative product is a broad definition under the copyright law and includes but is
  not limited to, copying any line of source code into your own program or linking
  (both statically or dynamically) to any part of this program.
The rights are not stated here as they are generally better known than the obligations.