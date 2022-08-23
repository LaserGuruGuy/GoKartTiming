# GoKartTiming

Analyze your GoKart race like a pro!

Read and parse PersonalRaceOverviewReport.pdf files and visualize absolute, integral and relative laptimes.

Drag-and-drop the pdf files on the main window or use the file menu to add one or more races.
Right-click  in the list of processed pdf race files to delete a race from the collection.
Select the View Menu Absolute Time, Integral Time and Relative Time to open the respective plot windows.
Select a race and select one or multiple teams to show their respective lap times.
The first selected team will be referenced to by the Relative Time window.
Add or remove one or more teams to/from the selection by pressing ctrl or shift together with the up/down keys or left mouse button.
Click a team in a Lap Time window Legenda to hide its line from the plot.
Right-click a Lap Time Window and save a snapshot as png file.

Work-in-progress: File Timing Tab and Live Timing Tab.
File Timing is the pdf list view where Live Timing embeds an edge webbrowser.

Pdf files parsed with iText7 from iText software under GNU Affero General Public License.
This adds a lot of bloat to the project but it gave me quick access to parsing pdf files.
