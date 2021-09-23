window.addEventListener("load", async function () {
    let display = new TestingDisplay(
        document.getElementById("questionNum"),
        document.getElementById("questionText"),
        document.getElementById("questionSubText")
    );
    let formManager = new FormManager(document.getElementById("formContainer"));
    formManager.register("confirm", new ConfirmForm("Далі"));
    formManager.register("Answer", new TextAnswerForm("Підтвердити", "Відповідь..."));
    let confirmForm = formManager.getForm("confirm");
    let timer = new TestingTimer(document.getElementById("timerDisp"));
    let sessionId = document.getElementById("scriptData").dataset.sessionid;
    let server = new Server("/api/Testing", Number.parseInt(sessionId));
    await server.updateState();
    timer.time = server.testState.timeLeft;
    timer.start();
    while (!timer.finished && !server.testState.finished) {
        display.update(server.testState.questionNum, server.testState.questionText, server.testState.questionSubText);
        formManager.hideForms();
        let aForm = formManager.getForm(server.testState.answerType);
        aForm.fill(server.testState.addition);
        let answer = await aForm.showAndGetAnswer();
        formManager.hideForms();
        let answerResult = await server.answer(answer);
        if (answerResult.isRight) {
            display.showMessage("Правильно!","");
        } else {
            display.showMessage("Неправильно!", `Правильна відповідь: ${answerResult.rightText}`);
        }
        await confirmForm.showAndGetAnswer();
        display.clear();
        await server.updateState();
        timer.time = server.testState.timeLeft;
    }
    window.location.href = `${document.getElementById("scriptData").dataset.hrefresult}/${server.testState.resultId}`;
});