The 335thUserCapture program is to simplify the way the 335th help desk backups and restores 
users.  Simplifying allows for an automated (no errors) in backing up users and allows techs 
to run the program and walk away.

The program currently backups users to wherever the program resides underneath the folder 
UserBackup.  It creates a datetime group and then sticks the Scanstate results there.
The intent is to stick the program on a remote shared drive.  The tech walks to a customers’ 
computer that needs to be backed up.  The tech navigates to the remote shared drive with 
the tool and double clicks it.  Once run, the tech selects the user and hits go.  Once 
complete, the user gets the new computer or the computer gets reimaged.  From there, the 
tech runs the tool again, selects restore user tab, selects the name that was backed and 
hits restore.  

--------------------------------------------

This program is a glorified GUI wrapper for Microsoft USMT scan state and load state 
(EX: http://technet.microsoft.com/en-us/library/hh825256.aspx) with the intended result 
being that it is extremely simple to use.  

Each tab is a independent user control.  Each tab retrieves a ViewController from a object 
factory called ViewModelFactory.  The ViewModelFactory class retrieves all the required 
dependencies from the ModelFactory as interfaces and passes them to the view model.  
Interfaces were used to adhere to the IoC design pattern 
(EX: http://joelabrahamsson.com/inversion-of-control-an-introduction-with-examples-in-net/ )  
This was used to be able to fully test all code to whatever degree is needed.

If you need to modify the code, please look at the Object Factory name space (and folder) and
follow the interfaces and their implementations.  Everything is relatively loosely coupled so 
extending should be relatively easy, though a bit verbose.

FEATURES TO IMPLEMENT AND ROADMAP
Create Scanstate Interface to remove hard dependency in CaptureOneUserOnComputer.cs
Show free space on backup drive and user folder size in CaptureOneUserOnComputer.xaml
Allow techs to delete backup jobs
Implement Reporting features 
	This includes jobs that were backup jobs that were deleted
Allow capturing all users from a external drive using the /offlinewindir option in
	scanstate
Redo the about page somehow

