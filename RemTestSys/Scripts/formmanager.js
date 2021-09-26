class FormManager {
    constructor() {
        this.forms = new Map();
    }
    register(formType, form) {
        this.forms.set(formType, form);
        form.hide();
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
