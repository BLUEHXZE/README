import * as React from "react"
import * as Final from "./Final"

type AppProps = {}
type AppState = {questions: Final.Question[]}

export const DummyQuestions: Final.Question[] = [
  // Open questions
  { kind: "open", id: 1, statement: "[DummyQuestion] Explain the benefits of client-side rendering in web applications.", answer: ""},
  { kind: "open", id: 2, statement: "[DummyQuestion] What is the purpose of Dependency Injection in software design?", answer: ""},
  { kind: "open", id: 3, statement: "[DummyQuestion] Describe the difference between REST and GraphQL APIs.", answer: ""},
  // Multiple-choice questions
  { kind: "mcq", id: 4, statement: "[DummyQuestion] Which of the following is a React Hook?",
    choices: ["useFetch", "useState", "useClass", "useHandler"], correctAnswer: 1, chosen: -1},
  { kind: "mcq", id: 5, statement: "[DummyQuestion] What is the default port for HTTP?",
    choices: ["80", "443", "8080", "3000"], correctAnswer: 0, chosen: -1},
  { kind: "mcq", id: 6, statement: "[DummyQuestion] Which HTTP method is idempotent?",
    choices: ["POST", "DELETE", "PUT", "OPTIONS"], correctAnswer: 2, chosen: -1},
];

export class App extends React.Component<AppProps, AppState> {
    constructor(props: AppProps) {
        super(props)
        this.state = {questions: DummyQuestions}
    }

    loadQuestions(){
      Final.fetchQuestions()
      .then(_ => _.kind == "some" ? _.value : [])
      .then(t => this.setState((s => ({...s, questions: t})))
      )
      .catch(_ => this.setState((s => ({...s, questions: []}))))
    }

    componentDidMount(): void {
      this.loadQuestions()
  }
    render(): React.ReactNode {

        return <div key="Exam">
            <h1>Web development Exam</h1>
            <div key="Welcome">
            <div>You can use this component to test your code. Place your own components inside the render of the App component.</div>
            <div>Comment/Uncomment (one by one is advised) the code (below) related to each respective component or question.</div> 
            <div>Please notice the components are called using specific props which should stay unchanged.</div>
            </div>
            <div key="Q2">
              <b>Question 2</b>
              <div key="Open">
              <i>Open question</i>
              <Final.QuizCard {...DummyQuestions[0]}/>
              </div>
              <div key="Mcq">
              <i>Mcq</i>
              <Final.QuizCard {...DummyQuestions[3]}/>
              </div>
            </div>
            <div key="Q3">
              <b>Question 3</b>
              <Final.Navigator Questions={DummyQuestions}/>  
            </div>
            <div key="Q4">
              <b>Question 4</b>
              <div>
                  { 
                    this.state.questions.map((_, i) => <div key={i} > {JSON.stringify(_)} </div>)
                  }
              </div>  
            </div>
            <div key="Q5">
              <b>Question 5</b>
              <Final.TestTypescript />
            </div>
        </div>
    }
}