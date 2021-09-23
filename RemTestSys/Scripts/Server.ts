class Server {
    url: string;
    testState: TestState;
    sessionId: number;
    constructor(url:string, sessionId: number) {
        this.url = url;
        this.sessionId = sessionId;
        this.testState = null;
    }
    async answer(answer:Answer):Promise<ResultOfAnswer>{
         return await this._postRequest(answer);
    }
    async updateState() {
        let resp = await fetch(`${this.url}/${this.sessionId}`);
        if (resp.ok) {
            this.testState = await resp.json();
        } else {
            throw new Error(resp.statusText);
        }
    }
    async _postRequest(dataObj: any) {
        let resp = await fetch(`${this.url}/${this.sessionId}`,
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(dataObj)
                }
            );
        if (resp.ok) {
            return await resp.json();
        } else {
            throw new Error(resp.statusText);
        }
    }
}
