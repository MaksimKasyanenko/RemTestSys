var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Server {
    constructor(url, sessionId) {
        this.url = url;
        this.sessionId = sessionId;
        this.testState = null;
    }
    answer(answer) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this._postRequest(answer);
        });
    }
    updateState() {
        return __awaiter(this, void 0, void 0, function* () {
            let resp = yield fetch(`${this.url}/${this.sessionId}`);
            if (resp.ok) {
                this.testState = yield resp.json();
            }
            else {
                throw new Error(resp.statusText);
            }
        });
    }
    _postRequest(dataObj) {
        return __awaiter(this, void 0, void 0, function* () {
            let resp = yield fetch(`${this.url}/${this.sessionId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dataObj)
            });
            if (resp.ok) {
                return yield resp.json();
            }
            else {
                throw new Error(resp.statusText);
            }
        });
    }
}
