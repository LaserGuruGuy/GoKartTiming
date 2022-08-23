function Driver(jsonDriver) {
    // "D" = DriverID
    var _identifier;
    this._identifier = jsonDriver["D"];
    Driver.prototype.setIdentifier = function (val) { this._identifier = val; }
    Driver.prototype.getIdentifier = function () { return this._identifier; }

    // "M" = WebMemberID
    var _webMemberID;
    this._webMemberID = jsonDriver["M"];
    Driver.prototype.setWebMemberID = function (val) { this._webMemberID = val; }
    Driver.prototype.getWebMemberID = function () { return this._webMemberID; }

    // "N" = DriverName
    var _name;
    this._name = jsonDriver["N"];
    Driver.prototype.setDriverName = function (val) { this._name = val; }
    Driver.prototype.getDriverName = function () { return this._name; }

    // "K" = KartNumber
    var _kartNumber;
    this._kartNumber = jsonDriver["K"];
    Driver.prototype.setKartNumber = function (val) { this._kartNumber = val; }
    Driver.prototype.getKartNumber = function () { return this._kartNumber; }

    // "P" = Position
    var _position;
    this._position = jsonDriver["P"];
    Driver.prototype.setPosition = function (val) { this._position = val; }
    Driver.prototype.getPosition = function () { return this._position; }

    // "L" = Laps
    var _laps;
    this._laps = jsonDriver["L"];
    Driver.prototype.setLap = function (val) { this._laps = val; }
    Driver.prototype.getLaps = function () { return this._laps; }

    // "T" = LastLapTimeMS
    var _lastLapTime;
    this._lastLapTime = jsonDriver["T"];
    Driver.prototype.setLapTime = function (val) { this._lastLapTime = val; }
    Driver.prototype.getLapTime = function () { return this._lastLapTime; }

    // "A" = AvarageLapTimeMS
    var _averageLapTime;
    this._averageLapTime = jsonDriver["A"];
    Driver.prototype.setAvarageLapTime = function (val) { this._averageLapTime = val; }
    Driver.prototype.getAvarageLapTime = function () { return this._averageLapTime; }

    // "B" = BestLapTimeMS
    var _bestLapTime;
    this._bestLapTime = jsonDriver["B"];
    Driver.prototype.setBestLapTime = function (val) { this._bestLapTime = val; }
    Driver.prototype.getBestLapTime = function () { return this._bestLapTime; }

    // "G" = gap
    var _gapTime;
    this._gapTime = jsonDriver["G"];
    Driver.prototype.setGapTime = function (val) { this._gapTime = val; }
    Driver.prototype.getGapTime = function () { return this._gapTime; }

    // "LP" = LastPassing
    //      0 = not the last passing
    //      1 = last passing
    var _isLastPassing;
    this._isLastPassing = jsonDriver["LP"];
    Driver.prototype.setLastPassing = function (val) { this._isLastPassing = val; }
    Driver.prototype.getLastPassing = function () { return this._isLastPassing; }

    // "R" = LastRecord
    var _lastRecord;
    this._lastRecord = jsonDriver["R"];
    Driver.prototype.setLastRecord = function (val) { this._lastRecord = val; }
    Driver.prototype.getLastRecord = function () { return this._lastRecord; }
}