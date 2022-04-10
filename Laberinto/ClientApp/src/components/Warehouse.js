import React, { Component } from 'react';

export class Warehouse extends Component {
static displayName = Warehouse.name;

  constructor(props) {
    super(props);
    this.state = { cells: [], loading: true };
  }

  componentDidMount() {
      this.populateWarehouseData('http://localhost:63011/cell');
  }

    static renderWarehouseTable(cells) {
        var output = [];

        for (var i = 7; i >= 0; i--) {
            output.push(<tr className='text-center'>
                {cells.filter(pilot => pilot.y === i)
                    .map(cell =>
                        <td style={{ backgroundColor: (cell.hall === false ? 'black' : "white") }}>
                            {(cell.hall === true ? cell.value : '')}
                        </td>
                    )
                }
            </tr>);
        }

        return (
          <table className='table' aria-labelledby="tabelLabel">
                <tbody>
                    {output}
            </tbody>
          </table>
        );
    }

    calculateDistances = (ev) => {
        ev.preventDefault();
        this.setState({ cells: [], loading: true });
        this.populateWarehouseData('http://localhost:63011/cell/CalculateDistances');
    }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Warehouse.renderWarehouseTable(this.state.cells);

    return (
      <div>
            <h1 id="tabelLabel" className="mt-4">Almac&eacute;n Aquiles</h1>
            <p>Haga clic en el bot&oacute;n para calcular las distancias.</p>
            <button className="btn btn-primary mb-4" onClick={this.calculateDistances}>Calcular distancias</button>
        {contents}
      </div>
    );
  }

    async populateWarehouseData(endpoint) {
        try
        {
            const response = await fetch(endpoint);
            const data = await response.json();
            this.setState({ cells: data, loading: false });
        }
        catch(error) {
            console.error(error);
        }
    }
}
