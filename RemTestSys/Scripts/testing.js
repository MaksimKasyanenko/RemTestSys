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
        let display = new TestingDisplay(document.getElementById("questionNum"), document.getElementById("questionText"), document.getElementById("questionSubText"), document.getElementById("questionCast"));
        let formManager = new FormManager();
        formManager.register("confirm", new ConfirmForm());
        let textAnswerForm = new TextAnswerForm();
        formManager.register("TextAnswer", textAnswerForm);
        formManager.register("OneOfFourVariantsAnswer", new OneOfFourVariantsAnswerForm());
        formManager.register("SomeVariantsAnswer", new SomeVariantsAnswerForm());
        formManager.register("SequenceAnswer", new SequenceAnswerForm());
        formManager.register("ConnectedPairsAnswer", new ConnectedPairsAnswerForm());
        formManager.hideForms();
        let confirmForm = formManager.getForm("confirm");
        let timer = new TestingTimer(document.getElementById("timerDisp"));
        let sessionId = document.getElementById("scriptData").dataset.sessionid;
        let server = new Server("/api/Testing", Number.parseInt(sessionId));
        yield server.updateState();
        timer.time = server.testState.timeLeft;
        timer.start();
        while (!server.testState.finished) {
            display.update(server.testState.questionNum, server.testState.questionText, server.testState.questionSubText, server.testState.questionCost);
            let aForm = formManager.getForm(server.testState.answerType);
            aForm.fill(server.testState.addition);
            let answer = yield aForm.showAndGetAnswer();
            aForm.hide();
            let answerResult = yield server.answer(answer);
            if (answerResult.isRight) {
                display.showMessage("Правильно!", "");
            }
            else {
                display.showMessage("Неправильно!", `Правильна відповідь:\n${answerResult.rightText}`);
            }
            yield confirmForm.showAndGetAnswer();
            confirmForm.hide();
            display.clear();
            yield server.updateState();
            timer.time = server.testState.timeLeft;
        }
        window.location.href = `${document.getElementById("scriptData").dataset.hrefresult}/${server.testState.resultId}`;
    });
});
