# Instructions

##### Prerequisites
You will need to install *Visual Studio* to run this program since it is written in *C#* with NUnit.

##### How to run

1. Clone this repo `git clone https://github.com/MilanMax/BackendTesing.git`
2. Open the *BackendTesting.sln* file from the project folder using *Visual Studio*.
3. After everything loads, in Test Explorer tab(window) click on the *run* icon in the top toolbar to start the tests.

##### Reporting

For the purpose of test reporting I have used *Allure* ( https://github.com/allure-framework ).
It is a test reporting tool.

###### How to use Allure

- First you will need to install it on your machine. You can do this by using *scoop*. To install *scoop* follow the quickstart instructions on this link.
- To install *allure* open up Powershell and run `scoop install allure`
- After all the tests have been run it will generate report files in a folder that is specified in the *allureConfig.json* file.
- Open up the *cmd*, navigate to that folder parent.(E.g If path in *allureConfig.json*  is "C:\\Users\\milan.maksimovic\\Desktop\\Zadatak\\Results"), go to Zadatak folder.
- Run `allure generate "Results" --clean`. This will generate the *allure-report* folder.
- Now run `allure open "allure-project"` and the test report should open up in your browser.
