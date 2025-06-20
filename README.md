# RaftLabAssessment
RaftLab Assessment

Instructions to Download, Configure, and Run the Application
1) Clone the Repository
	a) Clone the project to your local machine.
	b) Open the solution in Visual Studio and build the entire solution to restore dependencies and compile the code.

2) Configure Application Settings
	a) Open the appsettings.json file.
	b) Modify the configuration values as needed:
		I) CacheExpirationTime – Duration for which user data should be cached.
	   II) ClearCache – Set to true or false based on whether you want to clear cache at application startup.
	  III) UserId (optional) – Default user ID to use when no input is provided during runtime.


3) Run the Application
	a) Set RaftLabAssessment.Console as the startup project.
	b) You can run the application in either of the following ways:
		I) Via Visual Studio (Debug Mode): Using debug mode.
	   II) Via Executable: Build the solution, navigate to the output folder, and run the generated .exe file.

4) Provide User Input at Runtime
	a) When prompted, enter a user ID (numeric) to fetch specific user details.
	b) If you leave the input blank, the application will fetch the value from appsettings.json.
	
5) Check the logs for responses or any errors in the output directory (logs folder).

6) Execute Unit Tests (Optional)