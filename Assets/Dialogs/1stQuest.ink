VAR DialogueID = "001_GatherForSmith"
As you walk into the smithy you're greeted by a resting blacksmith. He notices you right away.
"Oy you need something?"

+"Gary forgot something for you and told me to come here."
 ->tellthetruth
+"I've reminded Gary that he was suppoused to bring you something."
 ->lieabit
 
 ===tellthetruth===
 "Oy that muppet forgot it again? How am I suppoused to work with him?"
 "What? Ya want a prize for that? Hmmm... I guess I have some old weapons laying around. Here you can choose one."
 ->Gary
 
 ===lieabit===
 "Hmm... This is the last time I'm working with him. I told you that."
 "What? Ya want a prize for that? Hmmm... I guess I have some old weapons laying around. Here you can choose one."
 ->Gary
===Gary===
 In this moment Gary enters the smithy. 
 "Hello Joseph I have the stuff you ordered."
 "You stupid donkey, you were suppoused to deliver it today morning!"
 "Calm down we were kinda busy kicking some asses at the tavern this morning."
 
 *"We?! I didn't see you helping out."
 
 He waves his hand at you.
 "The point is their asses were kicked."
  Blacksmith takes the package from Gary. He then turns to you. 
  "You look like yer poor. How bout running some errand for me?"
  
  You look at your empty coin purse. The money surely would be useful.
  ->Quest
  ===Quest===
  *"Okay what do you need me to do?"
  -> Questtalk
  ===Questtalk===
  "I need ya to gather me some good quality flints and some copper ore. The forest is full of it, just gather me some you muppet."
  "I can give you a few coins and I think I have something that you could use in the shop"
  You nod your head.
  *"I'll bring you the things you need. See you around."
    -> END
