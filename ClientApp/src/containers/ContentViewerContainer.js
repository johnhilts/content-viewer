import React, { Component } from 'react';
import ContentViewer from '../components/ContentViewer';

export class ContentViewerContainer extends Component {

  constructor(props) {
    super(props);
      this.handleContentClick = this.handleContentClick.bind(this);
      this.parseParentFolder = this.parseParentFolder.bind(this);
    this.state = { currentFolder: '', folders: [], files: [], loading: true, };
  }

  componentDidMount() {
    this.populateContentData();
  }

    parseParentFolder(folderName) {
        let folders = folderName.split('/').filter(f => f.length)
        if (!folders) {
            return '';
        }
        return folders.filter((f,i) => i < folders.length).reduce((a, f) => a + '/' + f)
    }

    handleContentClick(contentName, contentType, event) {
        event.preventDefault();
        this.setState({loading: true})
        // TODO use an extension
        const getParentFolder = contentName === '...'
        if (getParentFolder) {
            this.setState({currentFolder: this.parseParentFolder(contentName)})
        }
        this.rePopulateContentData(contentName, contentType)
    }

  render() {
    return (
      <ContentViewer
        onContentClick={this.handleContentClick}
        loading={this.state.loading}
        folders={this.state.folders}
        files={this.state.files}
        />
    )
  }

  async populateContentData() {
    const response = await fetch('api/content');
    const data = await response.json();
    this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, loading: false });
  }

    async rePopulateContentData(contentName, contentType) {
        const requestData = {CurrentFolderName: this.state.currentFolder, RequestFolderName: contentName, }
        const response = await fetch('api/content/folder', {method: 'PUT', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(requestData)});
        const data = await response.json();
        this.setState({ currentFolder: data.currentFolder, folders: data.folders, files: data.files, loading: false });
    }
}
