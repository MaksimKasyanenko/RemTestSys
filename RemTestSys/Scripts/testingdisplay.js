class TestingDisplay {
    constructor(questionNum, questionText, questionSubText, questionCast) {
        this.questionNum = questionNum;
        this.questionText = questionText;
        this.questionSubText = questionSubText;
        this.questionCast = questionCast;
    }
    update(questionNum, questionText, questionSubText, questionCast) {
        this.questionNum.textContent = questionNum.toString();
        this.questionText.textContent = questionText;
        this.questionSubText.textContent = questionSubText;
        this.questionCast.textContent = questionCast.toString();
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
