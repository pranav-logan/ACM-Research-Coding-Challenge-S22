# ACM Research Coding Challenge (Spring 2022)

## How to Compile and Run
Run the executable found in MushroomTester/bin/Release/net6.0 with the .NET 6 SDK installed. Alternatively, open MushroomTester.sln in Visual Studio 2022 (needs to be 2022 since older versions do not support .NET 6). 

## Explanation
For this problem, I decided to use the genetic algorithm to create my program. To numerically calculate whether a mushroom was edible, I created a class called Model that would hold an array of 22 weights (doubles) that would each correspond to a mushroom attribute in the csv file. 

Each Model would create a value for a mushroom by using the following: value = (weight a * attribute a) + (weight b * attribute b) + ... (last weight * last attribute). To make sure the value would always be between 0 – 100, all the weights sum up to 100% and the char values for the attributes are converted to ASCII values (since all uppercase letters fall between 65 to 90). Because the inputs for the attributes are always some number between 65 to 90, the program considers anything below 77.5 poisonous and anything above that edible.

Now that a Model can calculate a final score for a mushroom and give it either the label of “P” or “E” based on its weights, the program now creates an array 150 Models that all have randomly generated weight values. It then calculates how many mushrooms each Model guesses correctly on from a training set of 3500 mushrooms. The best model is then chosen as the parent of the second generation of Models. Every model of the new 150 Model array will derive its weights from the parent Model with some random deviation (+/- <5% difference). This process is repeated until either too many generations have passed, or the best scoring model is able to guess correctly on 99.5% of the training set.

After this, the best scoring model is applied to a completely different test set of 5000 mushrooms and is scored accordingly.        

## [](https://github.com/ACM-Research/-DRAFT-Coding-Challenge-S22#no-collaboration-policy)No Collaboration Policy

**You may not collaborate with anyone on this challenge.**  You  _are_  allowed to use Internet documentation. If you  _do_  use existing code (either from Github, Stack Overflow, or other sources),  **please cite your sources in the README**.

## [](https://github.com/ACM-Research/-DRAFT-Coding-Challenge-S22#submission-procedure)Submission Procedure

Please follow the below instructions on how to submit your answers.

1.  Create a  **public**  fork of this repo and name it  `ACM-Research-Coding-Challenge-S22`. To fork this repo, click the button on the top right and click the "Fork" button.

2.  Clone the fork of the repo to your computer using  `git clone [the URL of your clone]`. You may need to install Git for this (Google it).

3.  Complete the Challenge based on the instructions below.

4.  Submit your solution by filling out this [form](https://acmutd.typeform.com/to/uTpjeA8G).

## Assessment Criteria 

Submissions will be evaluated holistically and based on a combination of effort, validity of approach, analysis, adherence to the prompt, use of outside resources (encouraged), promptness of your submission, and other factors. Your approach and explanation (detailed below) is the most weighted criteria, and partial solutions are accepted. 

## [](https://github.com/ACM-Research/-DRAFT-Coding-Challenge-S22#question-one)Question One

[Binary classification](https://en.wikipedia.org/wiki/Binary_classification) is a type of classification task that labels elements of a set (i.e. dataset) into two different groups. An example of this type of classification would be identifying if people had a specific disease or not based on certain health characteristics. The dataset found in `mushrooms.csv` holds data (22 different characteristics, specifically) about different types of mushrooms, including a mushroom's cap shape, cap surface texture, cap color, bruising, odor, and more. Remember to split the data into test and training sets (you can choose your own percent split). Information about the meaning of the letters under each column can be found within the file `attributelegend.txt`.

**With the file `mushrooms.csv`, use an algorithm of your choice to classify whether a mushroom is poisonous or edible.**

**You may use any programming language you feel most comfortable. We recommend Python because it is the easiest to implement. You're allowed to use any library or API you want to implement this, just document which ones you used in this README file.** Try to complete this as soon as possible.

Regardless if you can or cannot answer the question, provide a short explanation of how you got your solution or how you think it can be solved in your README.md file. However, we highly recommend giving the challenge a try, you just might learn something new!
