
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
...,,,..........,,,...(@@@@@@@@@*,,,,...,,...........,,....,,,,,,,,,,,,,...,,,..
....................&@@@@@@@@@@@@@@,.........................,,,,,,,,,..........
...................#@@@@@@@@@@@@@@@@@.........................,,,,,,,...........
.................../@@@@@@@@@**@@@@@@@,......................,,,,,,,,,..........
.,..................,,@#*@   @@.   @@@@/..............,&@@@@@@@@@#,,,,,..,......
,,,,,.............,,,,#*&    &&      &@@#,(.......*@@@@@@@@@@@@@@@@@*,,,,,,,....
,,,,(/,,.......,,,,,,,#*(              @@&,..,@@@@@****@@@@@@@@@@@@@@(,,,,,,,,,,
,,,,,(@*,,,,,,,,,,,,,,#*&              .%#@&@@@.   @@@*      @@@@@@@@@,,,,,,,,,,
.,,.../@@*,,,,,,,,,.....,/@@@@*....,@@(***/%@@@               #@@@@@@/.,,,,....,
......./@@@,,,,@@#......,*@*...................#&../@@*      .@*(#%*............
........*@@@@@@@@@@**@@@@@#......................(.....&&   @**#................
.........*@@@@@@@@@@@@@@@@(.............................&@....,,................
....,,.,,,*@@&*#@@@@@@@@@@@/............................@@,,................,..,
..,,,,,,,,,,,,,,,@@#,,,,@@@@@@@(.......................@@@@@%.............,,,,,(
,,,,,,,,,,,,,,,,,,,,,,,,,@@(,..#@,..............*#..#@@@@@@@@@@@@@&..,,,,*&@@(,,
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#@@(........#@&/@@@@@@@@@@@@@@@@@@@/@@@@@%,,,,,
...,,,..........,,,....,,,,,,,,,,,,,,,#&@@&#,...../@%,,..(@@@@@@@@@@@@@@,..,,,..
.........................,,,,,,,,,.........................&&,,,,*@@@*..........
..........................,,,,,,,............................,,,,,,,,...........

    HEY STINKY!                
*** DOWNLOAD UNITY VERSION 2019.4.32f1 BEFORE DOWNLOADING THIS REPOSITORY!!! ***


--------------------------------------------------------------------------------


Welcome to the EGaDS 2020 Micro-jam! Hopefully, this size of this document doesn't alarm you; please understand that a lot of standards need to be met in order to combine the works of a multitude of game devs into a single unity project

This doc is mostly aimed at the programmers, but there are some things that the audio people need to see as well. For sounds, check the constraints list. For music, check the bottom.

Huge credit to Grant Ross, the EGaDS officer who spearheaded the first version of this jam in Fall 2020 and programmed the majority of the shared code for the main game.

If you have any questions, send a message in #ask-the-officers or pm one of the officers!.

Happy Jamming!

=========================================
Table of Contents:
    Requirements
    Constraints
    Navigating the Project
    Submission Checklist
=========================================

-----------------------------------------
   Requirements (what you need to do):
-----------------------------------------
 > EVERY SCRIPT you add must have the same unique namespace:
	///
	namespace MyTeam
	{
		public class MyClass : Monobehavior
		{
		// Your code here //

		}
	}
    ///
	- If you don't do this, there may be naming conflicts with other team's projects
	- Give the folder you submit the name: [team number]-[team name]. This is so that file names don't collide.
	    - Examples: 1-TheRibbiteers
	                6-Team 6
	                
                    <*****************************************>
 > DELETE all starting example files besides the main scene. DON'T reuse ANY of them
    - The purpose of these files is to show the you how to incorporate winning and losing into your game, as well as music and sound effects. 
    - If you reuse these objects, there will be GUID collisions with other projects that also reuse them, which causes bad things to happen.
    - List of Items to DELETE after you are done looking at them:
        - ExampleGameScript.cs
        - ExampleMusic.wav
        - win.wav
        - Minigame (Minigame scriptable object)
        - Example Game Object (In the Hierarchy window)
    - In addition, DON'T add components to the "Minigame Manager" object in the inspector. It should only have the MinigameManager script attached.    
    - Once you delete the Minigame scriptable object, you'll need to make another one. This is what the MinigameManager will use to get info about your game.  
        - Right click in the Project window.
        - Click "Create" and then "Minigame"
        - Drag your new minigame into the "Minigame" slot on the Minigame Manager in the inspector.
        - Change the name of the object to the name of your game at some point.
        - If you need help using the new Minigame object, check the Navigating the Project section

-----------------------------------------	
  Constraints List (what you can't do):
-----------------------------------------
 > Don't change ANYTHING in project settings. Don't add any new tags, collision layers, or sorting layers either.
    - There are a few added tags along with collision and rendering layers for you to work with. Hopefully these will be enough with how simple the games are.
    - You can check the collision matrix in the physics2D section of project settings, but again, don't change them.
	
                    <*****************************************>
 > Only play sounds using the Minigame scriptable object
    - This is so your sounds will be faded out on transition rather than cut off or continue playing
    - Play them by calling MinigameManager.Instance.PlaySound([sound name])
    - If you want a sound to loop, I recommend doing it using a coroutine that calls PlaySound at set intervals:
        ///
        private bool soundShouldPlay // set this elsewhere in code
        private IEnumerator PlayLoopingSound (string soundName) {
            while (soundShouldPlay) {
                MinigameManager.Instance.PlaySound(soundName);
                yield return new WaitForSeconds([looptime]);
            }
        }
        ///
        
                    <*****************************************>
 > Stick to WASD/arrow keys and space for your games controls. 
    - You can technically still call Input.GetKeyDown, but this may confuse the player. Keeping the controls simple makes the snappy-ness of these games possible
    
                    <*****************************************>
 > Don't use commands like "Timescale.time" or "SceneManager.Load"
    - Anything that alters the state of the game as a whole should be avoided.
    - A good rule-of-thumb is to only create code that you know will affect your minigame alone.
    - There's probably a whole slew of commands you could do that would break the main game. If you have questions about using any commands, ask in #ask-the-officers

                    <*****************************************>
 > Make sure that if you incorporate a canvas for UI, the settings for that canvas's scaler should match those of the main minigame canvas.
    - This makes sure that the UI you create matches up with the UI of the game as a whole.
    - Also make sure that you work in a 16:9 aspect ratio

-----------------------------------------
Navigating the Project:
-----------------------------------------
 > The assets folder of the project is split in half: Base Files and MyTeam.
    - As stated above, it is important that you rename the MyTeam folder to match your team's number and name.
    - DO NOT TOUCH ANYTHING IN Base Files.
         - If you are curious how things work, you are welcome to look inside, but DO NOT MAKE CHANGES.

                    <*****************************************>
 > Within the MyTeam folder
    - A copy of this README (Yay!)
    - The minigame scene. The name of the scene is important, as described below. 
    - A number of sample assets to show how things go together. Once you understand how they work, delete them.

                    <*****************************************>
 > The name of your scene is made into text that flashes before your game starts. The default name, you may notice, is "Scene".
    - Use this text to give some direction to the player. It should describe what the player needs to do in 1-3 words.
    - An exclamation mark is automatically added at the end of your scene name.
    - Changing the name of your scene shouldn't cause any loading issues, since the minigame scenes are loaded by numerical ID. Please don't edit the build settings.
    - This is not the title of your game. Make the name of your Minigame scriptable object the title.

                    <*****************************************>
 > The Minigame Manager
    - A Minigame Manager GameObject exists within your starting scene.
    - In the inspector, there are two fields you can change.
        - Debug Game Only: 
            - Set this to true when you want to test your minigame without the overhead of the main game.
            - You will need to set it to false on occasion to make sure that your minigame is incorporated well into the main game. 
                - For example, you will need to check if winning and losing work as you intended.
        - Minigame:
            - Set this to the minigame object that you create after deleting the example minigame.
    - In code, the Minigame Manager must be referenced to play sound clips and change victory state.
        - Call MinigameManager.Instance.PlaySound(SoundName) to play the appropriate sound clip from your minigame object
        - Set MinigameManager.Instance.minigame.gameWin to true or false as necessary.

                    <*****************************************>
 > The Minigame Object
    - Once you have deleted the starter files and created a new Minigame Object, it is important you know how to use it! 
    - The Minigame object has a number of fields in the inspector that you need to customize.
        - Game Time: How long your game will last. 
            - Short: 2 measures of 140 BPM music, roughly 3.6 seconds.
            -  Long: 4 measures of 140 BPM music, roughly 6.8 seconds.
        - Music: The music clip that will play during your game. Guidelines for music are below.
        - Volume: The volume scale of your music clip. This helps with mixing later on.
        - Sounds: List of sound effects that can be played during your game.
            -  Each added sound has a number of fields.
                - Sound Name: Name used by the MinigameManager to play the sound.
                - Clip: The audio file that will be played when this sound is selected.
                - Volume: The volume scale of your sound clip.
                - Min Pitch/Max Pitch: range of pitches at which the clip can play. Helps add variety if desired.
        - Game Win: This is how you signal to the MinigameManager that your game has been won or lost.
            - You can change whether the value starts at true or false in the inspector window.

                    <*****************************************>
 > You may notice that there is a green border attached to the Camera
    - This border is meant to contain your game, but dont be afraid to break out of it! 
    - The final border will be animated and be on Canvas sort order 1, so if you want to have some UI pop outside the border, set the canvas to level 2 or higher (but not above 99).
    
                    <*****************************************>
 > Music Making Guidelines: 
    - 140 BPM (to be in-time with the clock and main music)
    - Short games are 2 measures (~3.4 seconds)
    - Long games are 4 measures (~6.8 seconds)
    - I recommend that you leave about a half-beat of rest at the end so that the transition between songs is smooth.
    - By no means do you need to follow these guidelines exactly. The lengths are the only hard-limits.
-----------------------------------------
        SUBMISSION CHECKLIST:
-----------------------------------------
 > Did you delete the example objects listed above?
 > Did you rename the folder that has all of your assets from "MyTeam"?
 > Did you wrap all of your scripts in the same namespace?
 > Did you remove all of your debug.log and print statements? (so that the officers can debug easier)
 > Did you ONLY zip up YOUR scene and assets in your submission file?
 
 
 
 
 
 
 
 
 
 
                                                                                 
                                  %@@@@@@&  @@@@@@@*                            
                      ..    @@@@%&&((((((#@@@/(((((@@                           
                   @@%/((@&@(((((@((((((((%@((((((((@@&((%@@                    
                  &@(((((((@(((((%(((((((((#(((((((/@((((((&&&@@&@@@(           
            &@%((&@@(((((((#((((((%((((((((((((((((/%((((((#@(((((((@&          
           %@(((((((&(((((((((((((((((((((((((((((((((((((/&((((((((&&          
      .&@@@&@#((((((((&(((((((((((((((((((((((((((((((((((((((((((((@&&&#       
     @@%(((((&(((((((((((((((((((((((((((((((((((((((((((((((((((((%(((((@@     
    .@&((((((((%(((((((((((((((((((((((((((((((((((((((((((((((((((((((((#@.    
     &@((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((#@@,     
       &@#(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((@@/  
    @@#(((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((@@  
   @@/((((((((((((((((((((((((((((((%((((((((((((((((((((((((((((((((((((((%@/  
   %@%(((((((((((((((((((((((((&/((((&((((#((((((&/(%(((((((((((((((((&&&&@@@(  
    (@@&(/((((((((((((((((((((/@/((((%(((((%(((((&&((((((((((((((((((((((((((@@.
 *@@/((((((((((((((((((((((((((%%(((@@((((@@#(((&@/(((((((((((((((((((((((((((@&
 @@#((((((((((((((((((((((#/((((%@@&../&@&../@@@&@(((((@(((((((((((((((((((((@@ 
 .@@#((((((((((((((((((((((@((((&@               @@@@@#((((((((((((((((((#(%@&  
     /#@@((((((((((((((((((((&@&*@@@@@@/   ,@@@@@@* @@(((((((((((((((((((((((&@*
     #@((((((((((((((((((((((#@%  %@@&. %&@, %@@@/ % /@@/((((((((((((((((((((&@%
     %@&((((&@(((((((#((((&%@@##.%%#&#&(((((#%@#(&*/..@((((((((((((((@#((((%@@% 
           %&((((((#@%(((((@( %......&(((((((##.....%(@@@&((((((&(((((&@ ..     
           @@((((%@  @#((((@*@,.....&(((((((((%*.....@  %#((((((@@#(((@@,       
            (&&%.     %@@@&,#@@%.(%(((((((((((((%&.%@%.  @%((((@& .&@@&,   - Oh yeah, dude. That's all she wrote. 
                        &@&($$#&.@(((((((((((((((&*@#($(%@%*%%/                 Heh heh heh...
                     &@#($$$$$(@..,&@@&(((((#@@%,..@($$$$$$(@@                  
                   &@($$$$$$$$(@..@@%,&@&.#@%.(@@*.@($$$$$$$$(&@                
                 (@%($$$$$$$$$(@,.@%,..........,@*,@($$$$$$$$$$(@%              
                /@@@@@@#($$$$$(&(.@%............@*,@($$$$$$$(&@@@@#             
                      ,@%@&($$(%&/@#..,&,../&..,@&(&($$$(&&(@%                  
                     &@($$(#@(@@@@@@*..........@@@@@@&(&($$$(%@                 
                    @&($$(#@(&@@@@@*...../......@@@@@@#(@($$$$(@,               
                  #@(((((((((@@#((((&@@@@@@@@@%(((((%@@#($$$$$$(@&              
                  
