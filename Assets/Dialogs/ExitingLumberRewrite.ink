VAR DialogueID = "002_AfterTheFight"
VAR PlayerName = "Jack"

As you watch the bandits scatter, the lumberjack exits his cabin and looks at you.
"Thank you for taking care of them, but I'm afraid it wasn't the last time I saw them. The name is Andrey." #speaker: 

*"I'm [PlayerName], pleasure to meet you."
->Name
===Name===
"Great, now that we have pleasantries out of the way, tell me what made you come here." #speaker:Lumberjack

*"I was sent by a blacksmith to check up on you." #speaker: 

"So the old man sends you. I guess he wants this." #speaker:Lumberjack
He gives you the box full of wood. #speaker: 
"Go and give it to him. And be careful; there are more of these bandits in the woods. I just hope someone will take care of them for good." #speaker:Lumberjack
->Ending
===Ending===
*"I'll see what I can do about that. See you around." #speaker: 
    -> END