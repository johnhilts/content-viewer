import React, { Component } from 'react';

const renderContentTable = (contentInfo) => {
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
                      <td>
                          <button 
                            type="button"
                            className="link-button" 
                            onClick={() => this.setState({showSomething: true})}>
                              {content.name}
                          </button>
                      </td>
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

export default function ContentViewer(props) {
    let folders = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.folders, '123');

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, '456');

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
