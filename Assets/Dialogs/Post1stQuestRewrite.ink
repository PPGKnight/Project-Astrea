VAR DialogueID = "002_CheckLumbererForTheSmith"

"Ay, you're back. You got what I've asked for?" #speaker:Smith

*["Yes, I have your things here."]
 ->Givethings
 
 ===Givethings===
 "Hah, I knew I could count on you. Here, just as I promised, you can have one of my swords." #speaker:Smith #reward:item?Sword

 "Listen, kiddo, do you mind doing another thing for me?" #speaker:Smith
 
 *["What else do you want?"]
 ->Quest
 *["If you pay enough."]
 ->Quest
 ===Quest===
 "I have an agreement with the lumberjack. He was supposed to bring me fresh wood, but he hasn't been seen in the village for a few days. I want you to check his hut in the forest and see if he's still alive." #speaker:Smith
 *["I'll see what I can do."]
 ->DONE