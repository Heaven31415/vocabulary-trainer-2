#!/bin/bash

sourceDirectory=$1
targetDirectory=$2

cp $sourceDirectory/*.dll $targetDirectory
cp $sourceDirectory/*.exe $targetDirectory
cp $sourceDirectory/*.pdb $targetDirectory
cp $sourceDirectory/VocabularyTrainer2.deps.json $targetDirectory
cp $sourceDirectory/VocabularyTrainer2.runtimeconfig.json $targetDirectory
