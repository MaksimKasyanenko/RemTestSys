class Answer {
}

class ConfirmForm {
    constructor(textOfBtn) {
        this.generateForm(textOfBtn);
    }
    generateForm(arg) {
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add("hidden");
        this.form = document.createElement("form");
        let submitBtn = document.createElement("input");
        submitBtn.type = "submit";
        submitBtn.value = arg;
        this.form.append(submitBtn);
        this.htmlElement.append(this.form);
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                resolve(answer);
            };
        });
    }
    fill(additive) { }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}
class TextAnswerForm {
    constructor(textOfBtn, placeholder) {
        this.generateForm({ textOfBtn, placeholder });
    }
    generateForm(arg) {
        this.htmlElement = document.createElement("div");
        this.htmlElement.classList.add("hidden");
        this.form = document.createElement("form");
        let sbmt = document.createElement("input");
        sbmt.type = "submit";
        sbmt.value = arg.textOfBtn;
        this.input = document.createElement("input");
        this.input.placeholder = arg.placeholder;
        this.form.append(this.input);
        this.form.append(sbmt);
        let row = document.createElement("div");
        row.append(this.form);
        this.htmlElement.append(row);
    }
    showAndGetAnswer() {
        let answer = new Answer();
        this.htmlElement.classList.remove("hidden");
        return new Promise((resolve, reject) => {
            this.form.onsubmit = e => {
                e.preventDefault();
                answer.data = [this.input.value];
                resolve(answer);
            };
        });
    }
    fill(additive) { }
    hide() {
        this.htmlElement.classList.add("hidden");
    }
}

class FormManager {
    constructor(formContainer) {
        this.formContainer = formContainer;
        this.forms = new Map();
    }
    register(formType, form) {
        this.forms.set(formType, form);
        form.hide();
        this.formContainer.append(form.htmlElement);
    }
    hideForms() {
        for (let item of this.forms) {
            item[1].hide();
        }
    }
    getForm(answerType) {
        return this.forms.get(answerType);
    }
}

class ResultOfAnswer {
}

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

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
window.addEventListener("load", function () {
    return __awaiter(this, void 0, void 0, function* () {
        let display = new TestingDisplay(document.getElementById("questionNum"), document.getElementById("questionText"), document.getElementById("questionSubText"));
        let formManager = new FormManager(document.getElementById("formContainer"));
        formManager.register("confirm", new ConfirmForm("Далі"));
        formManager.register("Answer", new TextAnswerForm("Підтвердити", "Відповідь..."));
        let confirmForm = formManager.getForm("confirm");
        let timer = new TestingTimer(document.getElementById("timerDisp"));
        let sessionId = document.getElementById("scriptData").dataset.sessionid;
        let server = new Server("/api/Testing", Number.parseInt(sessionId));
        yield server.updateState();
        timer.time = server.testState.timeLeft;
        timer.start();
        while (!timer.finished && !server.testState.finished) {
            display.update(server.testState.questionNum, server.testState.questionText, server.testState.questionSubText);
            formManager.hideForms();
            let aForm = formManager.getForm(server.testState.answerType);
            aForm.fill(server.testState.addition);
            let answer = yield aForm.showAndGetAnswer();
            formManager.hideForms();
            let answerResult = yield server.answer(answer);
            if (answerResult.isRight) {
                display.showMessage("Правильно!", "");
            }
            else {
                display.showMessage("Неправильно!", `Правильна відповідь: ${answerResult.rightText}`);
            }
            yield confirmForm.showAndGetAnswer();
            display.clear();
            yield server.updateState();
            timer.time = server.testState.timeLeft;
        }
        window.location.href = `${document.getElementById("scriptData").dataset.hrefresult}/${server.testState.resultId}`;
    });
});

class TestingDisplay {
    constructor(questionNum, questionText, questionSubText) {
        this.questionNum = questionNum;
        this.questionText = questionText;
        this.questionSubText = questionSubText;
    }
    update(questionNum, questionText, questionSubText) {
        this.questionNum.textContent = questionNum.toString();
        this.questionText.textContent = questionText;
        this.questionSubText.textContent = questionSubText;
    }
    showMessage(text, subtext) {
        this.questionText.textContent = text;
        this.questionSubText.textContent = subtext;
    }
    clear() {
        this.questionNum.textContent = "";
        this.questionText.textContent = "";
        this.questionSubText.textContent = "";
    }
}

class TestingTimer {
    constructor(dispElement) {
        this.dispElement = dispElement;
        this.timeLeft = 0;
    }
    get time() {
        return this.timeLeft;
    }
    set time(val) {
        this.timeLeft = val < 0 ? 0 : val;
        this._updateDisp();
    }
    get finished() {
        return this.timeLeft <= 0;
    }
    start() {
        setTimeout(() => this.tick(), 1000);
    }
    tick() {
        this.time -= 1;
        this._updateDisp();
        if (this.finished)
            return;
        setTimeout(() => this.tick(), 1000);
    }
    _updateDisp() {
        let min = Math.floor(this.timeLeft / 60);
        let sec = this.timeLeft % 60;
        this.dispElement.textContent = `${min < 10 ? '0' : ''}${min} : ${sec < 10 ? '0' : ''}${sec}`;
    }
}

class TestState {
}
