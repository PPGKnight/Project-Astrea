VAR DialogueID = "TavernTutorialPreFight"
-> TheInn

=== TheInn ===
You wake up in a tavern. Your memories are hazy, and you can't remember what happened yesterday. Two drunk merchants argue near you.
"... my cabbage is the best in this town, and I will not let you insult my products!"
"Your cabbage is as rotten as your personality, you old geezer..."
In this moment, one of the mugs lands on your head, and you're drenched in low-quality ale.
+"What the hell?!"
    -> angery
+Tell them to calm down.
    -> TalkingItOut
+Ignore them.
    -> idc

=== angery ===
You stand up, wet and angry, yelling at the drunkards. They both turn red. Until one looks closer at your face.
"You're the one that destroyed my cabbage stand!"
You look at him, but you can't remember his face.
Both merchants look even angrier now. Prepare for a fight.
#fight:TavernTutorial1 #changeOutcome:0
->END

=== TalkingItOut ===
Standing up, you calmly take a cloth from the bartender, wiping your face. You join the conversation.
+Try to calm them down.
    ->calmingthestorm
+Join the Cabbage man.
    ->Cabbages4Eva
+Join the other Guy.
    ->FucktheCabbagesMan
    
=== calmingthestorm ===
Trying to calm them, the cabbage man loses it and tries to hit the other guy. He misses, and his punch lands on your face.
Now you're angry. Kick. Their. Asses. #fight:TavernTutorial1 #changeOutcome:0
->END

=== Cabbages4Eva ===
Joining the cabbage seller, he looks surprised, slowly recognizing your face. Then he seems to remember.
"You're the one that destroyed my cabbage stand!"
You look at him, but you can't remember his face.
Both merchants look even angrier. Prepare for a fight. #fight:TavernTutorial1 #changeOutcome:0
->END
===FucktheCabbagesMan===
Joining the other guy, you both insult the seller and his cabbages. He's starting to get red. After a while, he attacks you both. #fight:TavernTutorial1 #changeOutcome:1
->END

=== idc ===
Sitting unmoved, you observe. Unfortunately, the arguing men turn violent, and in a blink, you're caught in a fight. #fight:TavernTutorial1 #changeOutcome:0
->END