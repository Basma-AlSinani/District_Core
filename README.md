🧭 Start

Open Postman after running the project locally. You don’t need to log in at first — follow these steps in order.

1️⃣ Submit a Public Crime Report

Endpoint: POST /api/CrimeReport/public/submit-report

Purpose: Submit a crime report anonymously.

The system returns a ReportId and confirmation message (email sent if enabled).

2️⃣ Check Report Status

Endpoint: GET /api/CrimeReport/status/{id}public

Replace {id} with the report number from step 1.

3️⃣ Login as Admin

Endpoint: POST /api/Auth/login

Credentials:

Email: admin@crime.gov

Password: Admin123!

Copy the JWT token from the response for the next steps.

4️⃣ View All Reports

Endpoint: GET /api/CrimeReport/Get/All/Crime/Report

Use the token in headers (Authorization: Bearer ...).

5️⃣ Create New Case Linked to Report

Endpoint: POST /api/Cases/CreateNewCase

Purpose: Create a case linked with a specific ReportId.

The CreatedBy field is automatically taken from the token.

6️⃣ Assign Users to the Case

Endpoint: POST /api/CaseAssignees/assign

Purpose: Assign officer/investigator to a case.

Only authorized roles can perform this action.

7️⃣ Manage Evidence

Create text evidence: POST /api/Evidence/CreateTextEvidence

Create image evidence: POST /api/Evidence/CreateImageEvidence

Get image: GET /api/Evidence/GetImage/{id}

Update text: PUT /api/Evidence/UpdateTextEvidenceContent/{id}

Soft delete: DELETE /api/Evidence/SoftDelete/{id}

Hard delete: DELETE /api/Evidence/HardDelete/{id} (with confirmation)

8️⃣ Manage Case Comments

Add comment: POST /api/CaseComments/Add/{caseId}/{userId}

Get all comments: GET /api/CaseComments/GetAll/{caseId}

Delete comment: DELETE /api/CaseComments/Delete/{commentId}/{userId}

Comment text must be between 5–150 characters.

9️⃣ Verify Email Notifications

If email service is enabled, notifications are sent automatically for new reports, cases, or comments.

🔟 Review Permissions

Test endpoints with different user roles (Admin, Officer, Investigator) to confirm access control.
