function Heat(jsonData) {
    // "N" = HeatName
    var _heatName;
    this._heatName = jsonData["N"];
    Heat.prototype.setHeatName = function (val) { this._heatName = val; }
    Heat.prototype.getHeatName = function () { return this._heatName; }

    // "S" = State 
    //      0 = not started
    //      1 = running
    //      2 = pauzed
    //      3 = stopped
    //      4 = finished
    //      5 = next heat
    var _heatState;
    this._heatState = jsonData["S"];
    Heat.prototype.setHeatState = function (val) { this._heatState = val; }
    Heat.prototype.getHeatState = function () { return this._heatState; }

    // "E" = EndCondition
    //      0 = the heat needs to be finished manual
    //      1 = the heat finishes after X time
    //      2 = the heat finishes after X laps
    //      3 = the heat finished after X time or X laps. Depends on wich one is first
    var _endCondition;
    this._endCondition = jsonData["E"];
    console.log(this._endCondition);
    Heat.prototype.setEndCondition = function (val) { this._endCondition = val; }
    Heat.prototype.getEndCondition = function () { return this._endCondition; }

    // "R" = RaceMode
    //      0 = most laps wins
    //      1 = the best laptime is the winner
    //      2 = the best average time is the winner
    var _raceMode;
    this._raceMode = jsonData["R"];
    Heat.prototype.setRaceMode = function (val) { this._raceMode = val; }
    Heat.prototype.getRaceMode = function () { return this._raceMode; }

    // "D" = Drivers [array]
    var _driversArray;
    this._driversArray = jsonData["D"];
    var _drivers;
    this._drivers = [];
    for (var i = 0; i < this._driversArray.length; i++) {
        this._drivers[i] = new Driver(this._driversArray[i]);
    }
    Heat.prototype.setDrivers = function (val) { this._driversArray = val; }
    Heat.prototype.getDrivers = function () { return this._driversArray; }
    Heat.prototype.getDriverAtIndex = function (index) { return this._drivers[index]; }

    // "C" = Counter (in milliseconds)
    var _timeLeft;
    this._timeLeft = jsonData["C"];
    Heat.prototype.setTimeLeft = function (val) { this._timeLeft = val; }
    Heat.prototype.getTimeLeft = function () { return this._timeLeft; }

    // "CS" = ClockStarted
    //      0 = not started
    //      1 = started
    var _counterStarted;
    this._counterStarted = jsonData["CS"];
    Heat.prototype.setCounterStarted = function (val) { this._counterStarted = val; }
    Heat.prototype.getCounterStarted = function () { return this._counterStarted; }

    // "L" = RemainingLaps
    var _remainingLaps;
    this._remainingLaps = jsonData["L"];
    Heat.prototype.setRemainingLaps = function (val) { this._remainingLaps = val; }
    Heat.prototype.getRemainingLaps = function () { return this._remainingLaps; }

    // "T" = ActualHeatStart
    var _actualHeatStart;
    this._actualHeatStart = jsonData["T"];
    Heat.prototype.setActualHeatStart = function (val) { this._actualHeatStart = val; }
    Heat.prototype.getActualHeatStart = function () { return this._actualHeatStart; }
}