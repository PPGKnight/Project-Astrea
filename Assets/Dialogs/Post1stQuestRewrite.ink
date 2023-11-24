VAR DialogueID = "002_CheckLumbererForTheSmith"

"Ay, you're back. You got what I've asked for?"

*"Yes, I have your things here."
 ->Givethings
*"No, I don't have it yet."
 ->Donthavethem
 
 ===Givethings===
 "Hah, I knew I could count on you. Here, just as I promised, you can choose one thing from this."

After choosing an item:
 "Listen, kiddo, do you mind doing another thing for me?"
  
 *"What else do you want?"
 ->Quest
 *"If you pay enough."
 ->Quest
 ===Quest===
 "I have an agreement with the lumberjack. He was supposed to bring me fresh wood, but he hasn't been seen in the village for a few days. I want you to check his hut in the forest and see if he's still alive."
 *"I'll see what I can do."
 ->DONE
 
 ===Donthavethem===
 "Then what're ya waiting for? Go get them, then we're gonna talk."
 ->DONE