import React, { Component } from 'react';

export class ContentViewer extends Component {
  // static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { currentFolder: '', folders: [], files: [], loading: true };
  }

  componentDidMount() {
    this.populateContentData();
  }

  static renderContentTable(contentInfo) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Type</th>
          </tr>
        </thead>
        <tbody>
          {contentInfo.map(content =>
            <tr key={content.name}>
              <td>{content.name}</td>
              <td>{content.contentType}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : ContentViewer.renderContentTable(this.state.folders);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateContentData() {
    const response = await fetch('api/content');
    const data = await response.json();
    this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, loading: false });
  }
}
