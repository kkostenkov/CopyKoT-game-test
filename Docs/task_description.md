A test exercise as a variation of the King of Thieves game (a physics-based platformer).  

### DESCRIPTION: 
Please create a game with the following instructions:  

Main menu:  
- the main menu is an entry point to the game
- the main menu has a play button that redirects a user to the game scene  

Game scene:
- the game scene represents a maze with square blocks
- the left-bottom cell is always free
- the hero spawns at the center of the left-bottom cell
- the hero collides with floors and walls
- the hero runs in the right direction by default
- a tap will force the hero to jump; a tap is possible only if the hero has at least one contact
with a floor or a wall
- the hero stops running when they push against a wall
- if the hero slides on a wall a tap will force the hero not only to jump but to change running
direction as well
- each empty cell spawns a coin; there is a random spawn time after being collected
- the hero collects coins by touching them
- the game scene has a counter for collected coins
- the game scene has a countdown timer for collecting; when time runs out the game is over
Misc.:
- the game supports just English localization
- you could improve any aspect of the game to increase the user experience of players
- efficiency will undoubtedly add competitiveness, but it is better to remain satisfied with your
work than just be the first  

### REQUIREMENTS:  
- the project should be open via Unity (any latest version)
- the project should be written with C#
- there is the possibility of using free assets but not paid
- any external module/plugin/library/resource should be in the project
- the game should be launchable on Android (provide apk build)
- the result is a *.zip archive with everything inside

---
### Optional:
- the main menu shows the best coin result which has been ever achieved on this device
- there are rare coins chests that gives the hero 10% of his current coins amount; such chests
spawn much less often and have an expiration timer
- coins fly to the HUD after being collected

- use different solutions (shaders, ECS/DOTS, etc.) for demonstrating knowledge in that area

====================================================

P.S. The maze layout examples:
![Alt text](1.png?raw=true "1")
![Alt text](2.png?raw=true "1")