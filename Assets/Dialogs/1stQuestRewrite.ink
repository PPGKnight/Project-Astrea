VAR DialogueID = "001_GatherForSmith"
As you step into the smithy, a blacksmith at rest notices you immediately. #speaker: 

"Oy, you need something?" #speaker:Smith

*["Gary forgot something for you and told me to come here."]
->tellthetruth
*["I've reminded Gary that he was supposed to bring you something."]
->lieabit

===tellthetruth===
"Oy, that muppet forgot it again? How am I supposed to work with him?" #speaker:Smith
"What? Ya want a prize for that? Hmmm... I guess I have some old weapons laying around. Here you can choose one."
->Gary

===lieabit===
"Hmm... This is the last time I'm working with him. I told you that." #speaker:Smith
"What? Ya want a prize for that? Hmmm... I guess I have some old weapons laying around. Here you can choose one."
->Gary
===Gary===
In this moment, Gary enters the smithy. #speaker: 

"Hello Joseph, I have the stuff you ordered." #speaker:Gary
"You stupid donkey, you were supposed to deliver it today morning!" #speaker:Smith
"Calm down, we were kinda busy kicking some asses at the tavern this morning." #speaker:Gary

*["We?! I didn't see you helping out."]

He waves his hand at you. #speaker: 

"The point is their asses were kicked." #speaker:Gary
Blacksmith takes the package from Gary. He then turns to you. #speaker: 

"You look like yer poor. How 'bout running some errand for me?" #speaker:Smith

You look at your empty coin purse. The money surely would be useful. #speaker: 
->Quest
===Quest===
*["Okay, what do you need me to do?"]
-> Questtalk
===Questtalk===
"I need ya to gather me some good-quality flints and some copper ore. The forest is full of it, just gather me some, you muppet." #speaker:Smith
"I can give you a few coins, and I think I have something that you could use in the shop."
You nod your head. #speaker: 
*["I'll bring you the things you need. See you around."]
-> END