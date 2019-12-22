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
      const getContent = (contentInfo) => {
          const headerTitle = contentInfo[0].contentType === 0
          ? 'Folder'
          : 'File'
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>{headerTitle}</th>
                  </tr>
                </thead>
                <tbody>
                  {contentInfo.map(content =>
                    <tr key={content.name}>
                      <td><a href="#" onClick={alert(content.name)}>{content.name}</a></td>
                    </tr>
                  )}
                </tbody>
            </table>
        )
      }

    const contents = contentInfo.length
      ?
      getContent(contentInfo)
      :
      <div>&nbsp;</div>

    return contents
  }

  render() {
    let folders = this.state.loading
      ? <p><em>Loading...</em></p>
      : ContentViewer.renderContentTable(this.state.folders);

    let files = this.state.loading
      ? <p><em>Loading...</em></p>
      : ContentViewer.renderContentTable(this.state.files);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }

  async populateContentData() {
    const response = await fetch('api/content');
    const data = await response.json();
    this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, loading: false });
  }
}
