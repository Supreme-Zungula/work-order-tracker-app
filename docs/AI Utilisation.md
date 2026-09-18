## Tools
- OpenCode
- Gemini

## Prompts
Below is a list for AI prompts I used along the journey while implementing the project.

## Planning phase
**Models**: Gemini (browser)

**Prompt**: I am building a full-stack app using ASP.NET for backend and Vue 3 + Typescript for frontend. I am going to use a mono repo approach, I want you to generate a project folder architecture diagram for me. The backend will live in folder named 'work-order-api' and the frontend in 'work-order-vue-app'.

**Reason**: I wanted to generate the project folder architecture in markdown so that I can use it as input when I am setting up that project using OpenCode.

---

### Project setup.
**Models**: Union Alpha Free and Nemotron 3 Ultra free. (OpenCode)
**Input files**: `Project-folder-structure.md`

**Prompt**: I am building a full-stack project using ASP.NET Webapi for backend and Vue3 + typescript for the frontend. I want to generate project in the current folder. Follow the folder structure shown in @Project folder structure.md. Start with the ASP.NET project and create a pull request when you are done. The frontend project will be created afterwards.

 **Reason**: To speed up the setup process and Limiting the AI model to generating one project at a time and thus reduces chances of context loss and hallucination.

---

**Prompt**: Great! Now generate the vue 3 project in the same parent directory and name it 'work-order-vue-app' and configure typescript, formatting and Linting. Use pnpm as the the package manager and install the following packages:
	- Pinia
	- Tailiwindcss
	- Vuetify.
	
**Reason**: Limiting the AI agent to implementing the frontend project scaffolding only and without thinking about backend. And speed up the setup phase.

----


**Prompt**: Nice! Update the README file to reflect the changes and project setup and add instructions on how to run the project. Once done, create a PR from the current branch to main.


## Building phase
Below is a list of prompts I used while building the project.

#### Backend:
**Input files**: `API endpoints.md`

- **Prompt**: Create DTOs and controllers for the endpoints listed in @AI Endpoints.md file. Use the models provided in the models folder for references.
- **Prompt**: Add the CORS handling for the frontend app URL that is http://localhost:5172
- **Prompt**: Seed the database with some dummy data that I can use for testing the endpoints.

---

#### Frontend
- **Prompt**: Create a new form in time work-orders components folder. This Must be in a modal that is shown whenever the user clicks the add button on top on work orders table. Use script setup and tailwindcss for styling.
- **Prompt**: Now add a kebab menu item on the last column of each work order item. When click the kebab menu should have following options. 'Update order', 'View Activities' and 'Delete'.
- **Prompt**: Great! Now I want you to create a new page called WorkOrderActivities.vue. This page will be show when the use clicks 'View activities' option in the kebab menu. The page must have the work order title and description as the header of the page. All the work activites must be shown in an Vuetify table.
- **Prompt**: Create a Vuetify table in the ActivitiesView.vue that will display a list of all the work order activities. 
- **Prompt**:Update the README file to include the frontend stack, project structure and how to setup and run the project.