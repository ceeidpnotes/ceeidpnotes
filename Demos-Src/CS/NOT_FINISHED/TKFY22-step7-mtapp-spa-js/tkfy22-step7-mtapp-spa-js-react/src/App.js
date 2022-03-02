import { PageLayout } from "./components/PageLayout";
import React, { useState } from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { loginRequest } from "./authConfig";
import Button from "react-bootstrap/Button";

import { ProfileData,StringData } from "./components/ProfileData";
import { callMsGraph, callApiPermission01,callApiPermission02WebApi01,callApiPermission02WebApi02 } from "./graph";

function ProfileContentSimple() {
  const { instance, accounts, inProgress } = useMsal();
  const [accessToken, setAccessToken] = useState(null);

  const name = accounts[0] && accounts[0].name;
  const tenantId = accounts[0] && accounts[0].tenantId;

  function RequestAccessToken() {
      const request = {
          ...loginRequest,
          account: accounts[0]
      };

      // Silently acquires an access token which is then attached to a request for Microsoft Graph data
      instance.acquireTokenSilent(request).then((response) => {
          setAccessToken(response.accessToken);
      }).catch((e) => {
          instance.acquireTokenPopup(request).then((response) => {
              setAccessToken(response.accessToken);
          });
      });
  }

  return (
      <>
          <h5 className="card-title">Welcome {name} from TenantID: {tenantId}</h5>
          {accessToken ? 
              <p>Access Token Acquired!</p>
              :
              <Button variant="secondary" onClick={RequestAccessToken}>Request Access Token</Button>
          }
      </>
  );
};

function ProfileContent() {
  const { instance, accounts } = useMsal();
  const [graphData, setGraphData] = useState(null);

  const name = accounts[0] && accounts[0].name;

  function RequestProfileData() {
      const request = {
          ...loginRequest,
          account: accounts[0]
      };

      // Silently acquires an access token which is then attached to a request for Microsoft Graph data
      instance.acquireTokenSilent(request).then((response) => {
          callMsGraph(response.accessToken).then(response => setGraphData(response));
      }).catch((e) => {
          instance.acquireTokenPopup(request).then((response) => {
              callMsGraph(response.accessToken).then(response => setGraphData(response));
          });
      });
  }

  return (
      <>
          <h5 className="card-title">Welcome {name}</h5>
          {graphData ? 
              <ProfileData graphData={graphData} />
              :
              <Button variant="secondary" onClick={RequestProfileData}>Request Profile Information</Button>
          }
      </>
  );
};

function CustomApiPermission01() {
    const { instance, accounts } = useMsal();
    const [graphData, setGraphData] = useState(null);
  
    const name = accounts[0] && accounts[0].name;
  
    function RequestData() {
        const request = {
            ...loginRequest,
            scopes: ["api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission01"],
            account: accounts[0]
        };
  
        // Silently acquires an access token which is then attached to a request for Microsoft Graph data
        instance.acquireTokenSilent(request).then((response) => {
            callApiPermission01(response.accessToken).then(response => setGraphData(response));
        }).catch((e) => {
            instance.acquireTokenPopup(request).then((response) => {
                callApiPermission01(response.accessToken).then(response => setGraphData(response));
            });
        });
    }
  
    return (
        <>
            <h5 className="card-title">Welcome {name}</h5>
            {graphData ? 
                <StringData graphData={graphData} />
                :
                <Button variant="secondary" onClick={RequestData}>CustomApiPermission01</Button>
            }
        </>
    );
  };

  function CustomApiPermission02WebApi01() {
    const { instance, accounts } = useMsal();
    const [graphData, setGraphData] = useState(null);
  
    const name = accounts[0] && accounts[0].name;
  
    function RequestData() {
        const request = {
            ...loginRequest,
            scopes: ["api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission02.WebApi01"],
            account: accounts[0]
        };
  
        // Silently acquires an access token which is then attached to a request for Microsoft Graph data
        instance.acquireTokenSilent(request).then((response) => {
            callApiPermission02WebApi01(response.accessToken).then(response => setGraphData(response));
        }).catch((e) => {
            instance.acquireTokenPopup(request).then((response) => {
                callApiPermission02WebApi01(response.accessToken).then(response => setGraphData(response));
            });
        });
    }
  
    return (
        <>
            <h5 className="card-title">Welcome {name}</h5>
            {graphData ? 
                <StringData graphData={graphData} />
                :
                <Button variant="secondary" onClick={RequestData}>ApiPermission02WebApi01</Button>
            }
        </>
    );
  };
  
  function CustomApiPermission02WebApi02() {
    const { instance, accounts } = useMsal();
    const [graphData, setGraphData] = useState(null);
  
    const name = accounts[0] && accounts[0].name;
  
    function RequestData() {
        const request = {
            ...loginRequest,
            scopes: ["api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission02.WebApi02"],
            account: accounts[0]
        };
  
        // Silently acquires an access token which is then attached to a request for Microsoft Graph data
        instance.acquireTokenSilent(request).then((response) => {
            callApiPermission02WebApi02(response.accessToken).then(response => setGraphData(response));
        }).catch((e) => {
            instance.acquireTokenPopup(request).then((response) => {
                callApiPermission02WebApi02(response.accessToken).then(response => setGraphData(response));
            });
        });
    }
  
    return (
        <>
            <h5 className="card-title">Welcome {name}</h5>
            {graphData ? 
                <StringData graphData={graphData} />
                :
                <Button variant="secondary" onClick={RequestData}>ApiPermission02WebApi02</Button>
            }
        </>
    );
  };  

  //https://graph.microsoft.com/v1.0/me/joinedTeams | Team.ReadBasic.All
  //
function App() {
  return (
      <PageLayout>
          <AuthenticatedTemplate>
              <p>You are signed in!</p>
              <ProfileContentSimple/>
              <br/>
              <ProfileContent/>
              <br/>
              <CustomApiPermission01/>
              <br/>
              <CustomApiPermission02WebApi01/>
              <br/>
              <CustomApiPermission02WebApi02/>
              <br/>
          </AuthenticatedTemplate>
          <UnauthenticatedTemplate>
              <p>You are not signed in! Please sign in.</p>
          </UnauthenticatedTemplate>
          <h3>Consent for API (first!)</h3>
          <p>
          <a target="_blank" href="https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=d639132a-a25d-49ea-b5c3-2f8cb62e5615&redirect_uri=https://localhost:7103&state=http://localhost:3000&scope=https://graph.microsoft.com/.default">Link to Consent (Api, for https://graph.microsoft.com/.default)</a>
          </p>
          <h3>Consent for Client</h3>
          <p>
          <a href="https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=ced1687a-1072-4a0f-acd2-71a4f5f8d256&redirect_uri=http://localhost:3000&state=ASD123&scope=api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/.default">Link to Consent (.default)</a>
          </p>
          <p>
          <a href="https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=ced1687a-1072-4a0f-acd2-71a4f5f8d256&redirect_uri=http://localhost:3000&state=ASD123&scope=api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission01 api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission02.WebApi01 api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission02.WebApi02">Link to Consent (full)</a>
          </p>
          <p>
          <a href="https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=ced1687a-1072-4a0f-acd2-71a4f5f8d256&redirect_uri=http://localhost:3000&state=ASD123&scope=api://d639132a-a25d-49ea-b5c3-2f8cb62e5615/ApiPermission01">Link to Consent (only ApiPermission01)</a>
          </p>
      </PageLayout>
  );
}

export default App;