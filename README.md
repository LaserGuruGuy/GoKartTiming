# GoKartTiming

Tool to scan PersonalRaceOverviewReport.pdf files and visualize absolute, integral and relative laptimes.
Drag-and-drop the pdf files on the main window or use the file menu to add one or more races.
Select the View Menu Absolute Time, Integral Time and Relative Time to open the respective windows.
Select a race and next one or multiple teams to show their respective lap times.
The first selected team will be referenced to by the Relative Time window.
Add or remove one or more teams to/from the selection by pressing ctrl or shift together with the up/down keys or left mouse button.

Work-in-progress: File Timing Tab and Live Timing Tab.
File Timing is the pdf list view where Live Timing embeds an edge webbrowser.

Pdf files parsed with iText7 from iText software under GNU Affero General Public License.
This adds a lot of bloat to the project but it gave me quick access to parsing pdf files.
