VAR DialogueID = "TavernTourWithGary"
As you both exit the tavern, you find yourselves in the middle of a small village. Gary steps before you and says, "Let's start with the main market." #teleport:MainPlayer_HendleyMarket
Gary leads you to one of the buildings on the main square, where the sound of metal hitting metal fills the air. #teleport:MainPlayer_HendleySmith
"This is the Smith's workshop. If you ever need your weapons fixed or sharpened, you just need to come here." #speaker:Gary
 
Approaching the smithy, you see the blacksmith crafting some kind of hatchet. Gary seems to gasp when you get close to the building. #speaker: 

+["What happened?"]
-> Package
+["You forgot something didn't you?"]
-> Package
+[Stay silent]
-> Package

===Package====
"I forgot about a package that needed to be delivered to the Smith. I'll need to go back and grab it after I show you around. Let's get going." #speaker:Gary

He then takes you to the next location. #teleport:MainPlayer_HendleyMainGate #speaker: 
"This is the Main gate; if you need to exit the village, this is the way. #speaker:Gary

Next, I'll show you the pride of this village. #teleport:MainPlayer_HendleyOldTree
This is the old Oak tree, planted here when the village was founded."

As you move closer to the tree, you notice that the buildings here start to get much more expensive. Gary seems to see your reaction and says, #teleport:MainPlayer_HendleyDistrict #speaker: 
"We're entering the rich district. Here you can see the holy temple of Lune. If you ever need help from the divine, this is the place." #speaker:Gary

"That's everything, to be honest. It's not the biggest town, but it's ours. Let's get back to the Inn so I can deliver the things for the Smith. I'll tell you what, friend, I'm going to get my package from the tavern, and I'll meet you at the smithy. Good? Good. See you there."

-> END