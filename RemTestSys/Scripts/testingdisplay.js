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
