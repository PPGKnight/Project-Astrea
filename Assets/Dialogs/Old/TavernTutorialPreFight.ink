VAR DialogueID = "TavernTutorialPreFight"
-> TheInn

=== TheInn ===
You wake up in a tavern. Your memories are hazy, and you can't remember what happened yesterday. You see two drunk merchants arguing right next to you.
"... my cabbage is the best in this town and I will not let you insult my products!"
"Your cabbage is as rotten as your personality you old geezer..."
In this moment one of the mugs lands on your head and You're drenched in low quality Ale.
+"What the hell?!"
    -> angery
+Tell them to calm down.
    -> TalkingItOut
+Ignore them.
    -> idc

=== angery ===
You stand up from your table, wet and angry you start to yell at the drunkies. They both get all red on their faces. Until one of them starts to look closer on your face.
"You're the one that destroyed my cabbage stand!"
You look at him, but you can't remember his face.
Both merchants look even more angry now. You need to prepare for a fight. #fight:TavernTutorial1 #changeOutcome:0
-> END

=== TalkingItOut ===
As you stand up from your table, you calmly take some kind of cloth from bartender and you wipe your face with it. You look at the arguing men and you join the conversation.
+Try to calm them down.
    ->calmingthestorm
+Join the Cabbage man.
    ->Cabbages4Eva
+Join the other Guy.
    ->FucktheCabbagesMan
    
=== calmingthestorm ===
As you try to calm them both with your words. The cabbage man doesn't handle the situation too well and he tries to hit the other guy. He misses and his punch lands on your face.
Now you're angry. Kick. Their. Asses. #fight:TavernTutorial1 #changeOutcome:0
-> END

=== Cabbages4Eva ===
You join the conversation on the side of the cabbage seller. He looks suprised and he slowly starts to look at your face confused. Then he seems like he remembered something.
"You're the one that destroyed my cabbage stand!"
You look at him, but you can't remember his face.
Both merchants look even more angry now. You need to prepare for a fight. #fight:TavernTutorial1 #changeOutcome:0
-> END

===FucktheCabbagesMan===
You join the conversation on the side of the other guy. You both start to insult the seller and his cabbages. He's starting to get red. After a while his patience is gone and he attacks you both. #fight:TavernTutorial2 #changeOutcome:1
-> END

=== idc ===
As you sit unmoved by that mug of Ale, you start to look around the tavern. Unfortunately the arguing men start to get violent and in a blink of an eye you're catch in a fight. #fight:TavernTutorial1 #changeOutcome:0
-> END
