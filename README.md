# Vocabulary Trainer 2

`Vocabulary Trainer 2` is an CLI program that helps you to learn German
vocabulary in more efficient manner by applying a simple spaced repetition algorithm

## How does the spaced repetition algorithm works?

Every added word is converted into a set of flashcards. Every flashcard has a
cooldown and you won't be able to practice it until enough time has passed.

Initial cooldown is set to 12 hours (configurable in `Config.json`), because
it doesn't make much sense to practice something which you have already practiced
by adding it to the program.

After waiting for 12 hours you will be able to practice a flashcard.

If your answer is correct its cooldown will increase from 1 day to 2 days
up to a maximum of 32 days (configurable in `Config.json`), every time the
cooldown will be multiplied by 2.

On the other hand if your answer is incorrect its cooldown will decrease from
32 to 16, 8, 4 days and so on until the minimum of 1 day.
